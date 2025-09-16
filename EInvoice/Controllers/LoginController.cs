using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Security.Claims;
using PresentationLayer.Models.ApiResponses;
using System.Net.Http.Headers;

public class LoginController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LoginController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPasswordSubmit([FromBody] ForgotPasswordSubmitModel payload)
    {
        if (payload is null)
            return BadRequest(new { success = false, message = "Geçersiz istek" });

        var client = _httpClientFactory.CreateClient("Api");
        var apiRes = await client.PostAsJsonAsync("/Credential/ChangePassword", new
        {
            Email = payload.Email,
            IdentityNumber = payload.IdentityNumber,
            FullName = payload.FullName,
            NewPassword = payload.NewPassword
        });

        var data = await apiRes.Content.ReadFromJsonAsync<GenericMessageResponse>();
        if (!apiRes.IsSuccessStatusCode || data == null)
        {
            var raw = await apiRes.Content.ReadAsStringAsync();
            return UnprocessableEntity(new { success = false, message = data?.Message ?? raw ?? "İşlem başarısız" });
        }

        return Ok(new { success = true, message = data.Message ?? "Parola sıfırlandı. Yeni şifre ile giriş yapabilirsiniz." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginUser([FromBody] LoginViewModel model)
    {
        var client = _httpClientFactory.CreateClient("Api");
        // API'ye sadece gerekli alanları gönder
        var apiRes = await client.PostAsJsonAsync("/Auth/login", new { model.Email, model.Password });

        if (!apiRes.IsSuccessStatusCode)
            return BadRequest(new { message = "Giriş başarısız" });

        var data = await apiRes.Content.ReadFromJsonAsync<LoginResponse>();
        if (data is null || string.IsNullOrEmpty(data.AccessToken))
            return BadRequest(new { message = "Token al�namad�." });

        // 1) Tokenlar� Session�da tut (API �a�r�lar�nda kullanaca��z)
        HttpContext.Session.SetString("AccessToken", data.AccessToken);
        HttpContext.Session.SetString("RefreshToken", data.RefreshToken ?? "");

        // 2) Kullanıcının ad soyad bilgisini al ve claim olarak ayarla
        string fullName = model.Email;
        try
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.AccessToken);
            var meResponse = await client.GetAsync("/Credential/Me");
            if (meResponse.IsSuccessStatusCode)
            {
                var me = await meResponse.Content.ReadFromJsonAsync<GetMyCredentialResponse>();
                if (me != null && !me.Error)
                {
                    var userRes = await client.GetAsync($"/User/WithPerson/{me.UserId}");
                    if (userRes.IsSuccessStatusCode)
                    {
                        var user = await userRes.Content.ReadFromJsonAsync<GetUserWithPersonByIdResponse>();
                        if (user != null && !user.Error)
                        {
                            fullName = user.PersonName;
                        }
                    }
                }
            }
        }
        catch
        {
            // isim alınamazsa e-posta gösterilir
        }

        // 3) Cookie auth ile oturum aç (Name: Ad Soyad, Email ayrı claim)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "uid:" + (User?.Identity?.Name ?? model.Email)),
            new Claim(ClaimTypes.Name, fullName),
            new Claim(ClaimTypes.Email, model.Email)
        };
        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = false,
            ExpiresUtc = null,
            AllowRefresh = false
        };
        await HttpContext.SignInAsync("Cookies", principal, authProperties);

        // 4) AJAX'e başarılı JSON dön -> client tarafı Dashboard'a yönlendirsin
        return Ok(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Remove("AccessToken");
        HttpContext.Session.Remove("RefreshToken");
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost()
    {
        HttpContext.Session.Remove("AccessToken");
        HttpContext.Session.Remove("RefreshToken");
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Index");
    }
}