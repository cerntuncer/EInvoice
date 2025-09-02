using EInvoice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using PresentationLayer.Models;
using PresentationLayer.Models.ApiRequests;
using PresentationLayer.Models.ApiResponses;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace EInvoice.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly ILogger<UserInfoController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserInfoController(ILogger<UserInfoController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var meResponse = await client.GetAsync("/Credential/Me");
            if (!meResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Kullanıcı bilgileri alınamadı. Lütfen tekrar giriş yapın.";
                return View(model: null);
            }
            var me = await meResponse.Content.ReadFromJsonAsync<GetMyCredentialResponse>();
            if (me == null || me.Error)
            {
                TempData["ErrorMessage"] = me?.Message ?? "Kullanıcı bilgileri alınamadı.";
                return View(model: null);
            }

            var userResponse = await client.GetAsync($"/User/WithPerson/{me.UserId}");
            if (!userResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Profil bilgileri getirilemedi.";
                return View(model: null);
            }
            var user = await userResponse.Content.ReadFromJsonAsync<GetUserWithPersonByIdResponse>();
            if (user == null || user.Error)
            {
                TempData["ErrorMessage"] = user?.Message ?? "Profil bilgileri getirilemedi.";
                return View(model: null);
            }

            var model = new ProfileViewModel
            {
                UserId = user.UserId,
                UserType = user.UserType,
                UserStatus = user.UserStatus,
                PersonId = user.PersonId,
                Name = user.PersonName,
                IdentityNumber = user.IdentityNumber,
                TaxOffice = user.TaxOffice ?? string.Empty,
                PersonType = user.PersonType,
                PersonStatus = user.PersonStatus,
                Email = me.Email
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] ProfileViewModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
                return BadRequest(new { success = false, message = "Ad Soyad zorunludur." });

            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized(new { success = false, message = "Oturum süresi doldu." });
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var meResponse = await client.GetAsync("/Credential/Me");
            if (!meResponse.IsSuccessStatusCode)
            {
                return Unauthorized(new { success = false, message = "Kullanıcı doğrulanamadı." });
            }
            var me = await meResponse.Content.ReadFromJsonAsync<GetMyCredentialResponse>();
            if (me == null || me.Error)
            {
                return Unauthorized(new { success = false, message = me?.Message ?? "Kullanıcı doğrulanamadı." });
            }

            var userResponse = await client.GetAsync($"/User/WithPerson/{me.UserId}");
            if (!userResponse.IsSuccessStatusCode)
            {
                return StatusCode(500, new { success = false, message = "Mevcut profil getirilemedi." });
            }
            var current = await userResponse.Content.ReadFromJsonAsync<GetUserWithPersonByIdResponse>();
            if (current == null || current.Error)
            {
                return StatusCode(500, new { success = false, message = current?.Message ?? "Mevcut profil getirilemedi." });
            }

            // Kimlik Tipi (PersonType) ve Kullanıcı Tipi (UserType) güncellenmez.
            var personUpdateRequest = new UpdatePersonRequest
            {
                Id = current.PersonId,
                Name = model.Name,
                IdentityNumber = current.IdentityNumber,
                TaxOffice = model.TaxOffice ?? string.Empty,
                Type = current.PersonType,
                Status = current.PersonStatus
            };

            var personResponse = await client.PutAsJsonAsync("/Person", personUpdateRequest);
            if (!personResponse.IsSuccessStatusCode)
            {
                return BadRequest(new { success = false, message = "Kişi bilgileri güncellenemedi." });
            }

            // Kullanıcının cookie claim'indeki ad bilgisini güncelle
            try
            {
                // Güncel kullanıcı adını tekrar çek
                var refreshedUserRes = await client.GetAsync($"/User/WithPerson/{me.UserId}");
                if (refreshedUserRes.IsSuccessStatusCode)
                {
                    var refreshedUser = await refreshedUserRes.Content.ReadFromJsonAsync<GetUserWithPersonByIdResponse>();
                    var newFullName = refreshedUser?.PersonName ?? model.Name ?? User?.Identity?.Name;

                    var currentClaims = User?.Claims?.ToList() ?? new List<Claim>();
                    var nameId = currentClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? ("uid:" + (User?.Identity?.Name ?? me.Email));
                    var email = currentClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? me.Email;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, nameId),
                        new Claim(ClaimTypes.Name, newFullName ?? email),
                        new Claim(ClaimTypes.Email, email)
                    };

                    var identity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(identity);
                    var authProps = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = null,
                        AllowRefresh = false
                    };
                    await HttpContext.SignInAsync("Cookies", principal, authProps);
                }
            }
            catch { /* claim yenileme başarısız olsa bile sessiz geç */ }

            return Ok(new { success = true, message = "Bilgileriniz başarıyla güncellendi." });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordPayload payload)
        {
            if (payload == null || string.IsNullOrWhiteSpace(payload.CurrentPassword) || string.IsNullOrWhiteSpace(payload.NewPassword))
            {
                return BadRequest(new { success = false, message = "Geçersiz istek." });
            }

            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized(new { success = false, message = "Oturum süresi doldu." });
            }
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var apiResponse = await client.PostAsJsonAsync("/Credential/ChangePassword", new
            {
                currentPassword = payload.CurrentPassword,
                newPassword = payload.NewPassword
            });
            if (!apiResponse.IsSuccessStatusCode)
            {
                var err = await apiResponse.Content.ReadFromJsonAsync<dynamic>();
                var msg = err?.message?.ToString() ?? "Şifre değiştirilemedi.";
                return BadRequest(new { success = false, message = msg });
            }
            var ok = await apiResponse.Content.ReadFromJsonAsync<dynamic>();
            return Ok(new { success = true, message = (string?)ok?.message ?? "Şifre güncellendi." });
        }
    }

    public class ChangePasswordPayload
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}