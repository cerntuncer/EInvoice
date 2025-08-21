using System.Diagnostics;
using EInvoice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ApiResponses;
using System.Net.Http.Headers;
using PresentationLayer.Models;
using System.Net.Http.Json;
using PresentationLayer.Models.ApiRequests;

namespace EInvoice.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardController(ILogger<DashboardController> logger, IHttpClientFactory httpClientFactory)
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

            // 1) Me: email'den UserId al
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

            // 2) User detayını çek
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

            var model = new DashboardProfileViewModel
            {
                UserId = user.UserId,
                UserType = user.UserType,
                UserStatus = user.UserStatus,
                PersonId = user.PersonId,
                Name = user.PersonName,
                IdentityNumber = user.IdentityNumber,
                TaxOffice = user.TaxOffice ?? string.Empty,
                PersonType = user.PersonType,
                PersonStatus = user.PersonStatus
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(DashboardProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            // Kişi güncelle
            var personUpdateRequest = new UpdatePersonRequest
            {
                Id = model.PersonId,
                Name = model.Name,
                IdentityNumber = model.IdentityNumber,
                TaxOffice = model.TaxOffice ?? string.Empty,
                Type = model.PersonType,
                Status = model.PersonStatus
            };

            var personResponse = await client.PutAsJsonAsync("/Person", personUpdateRequest);
            if (!personResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Kişi bilgileri güncellenemedi.";
                return View("Index", model);
            }

            // Kullanıcı güncelle (tip/durum)
            var userUpdateRequest = new UpdateUserRequest
            {
                Id = model.UserId,
                Type = model.UserType,
                Status = model.UserStatus
            };

            var userResponse = await client.PutAsJsonAsync("/User/update", userUpdateRequest);
            if (!userResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Kullanıcı bilgileri güncellenemedi.";
                return View("Index", model);
            }

            TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}