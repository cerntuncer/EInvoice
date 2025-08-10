using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Net.Http.Json;
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
            return BadRequest(new { message = "Giriþ baþarýsýz" });

        var data = await apiRes.Content.ReadFromJsonAsync<LoginResponse>();
        if (data is null || string.IsNullOrEmpty(data.AccessToken))
            return BadRequest(new { message = "Token alýnamadý." });

        // 1) Tokenlarý Session’da tut (API çaðrýlarýnda kullanacaðýz)
        HttpContext.Session.SetString("AccessToken", data.AccessToken);
        HttpContext.Session.SetString("RefreshToken", data.RefreshToken ?? "");

        // 2) Cookie auth ile oturum aç
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "uid:" + (User?.Identity?.Name ?? model.Email)),
        new Claim(ClaimTypes.Name, model.Email),
        new Claim(ClaimTypes.Email, model.Email)
    };
        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync("Cookies", principal);

        // 3) AJAX’e baþarýlý JSON dön – client tarafý Dashboard’a yönlendirsin
        return Ok(new { success = true });
    }

}
