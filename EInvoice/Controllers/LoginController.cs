using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Security.Claims;

public class LoginController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LoginController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginUser([FromBody] LoginViewModel model)
    {
        var client = _httpClientFactory.CreateClient("Api");
        var apiRes = await client.PostAsJsonAsync("/Auth/login", model);

        if (!apiRes.IsSuccessStatusCode)
            return BadRequest(new { message = "Giriş başarısız" });

        var data = await apiRes.Content.ReadFromJsonAsync<LoginResponse>();
        if (data is null || string.IsNullOrEmpty(data.AccessToken))
            return BadRequest(new { message = "Token al�namad�." });

        // 1) Tokenlar� Session�da tut (API �a�r�lar�nda kullanaca��z)
        HttpContext.Session.SetString("AccessToken", data.AccessToken);
        HttpContext.Session.SetString("RefreshToken", data.RefreshToken ?? "");

        // 2) Cookie auth ile oturum a�
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "uid:" + (User?.Identity?.Name ?? model.Email)),
        new Claim(ClaimTypes.Name, model.Email),
        new Claim(ClaimTypes.Email, model.Email)
    };
        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
            AllowRefresh = false
        };
        await HttpContext.SignInAsync("Cookies", principal, authProperties);

        // 3) AJAX�e ba�ar�l� JSON d�n � client taraf� Dashboard�a y�nlendirsin
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