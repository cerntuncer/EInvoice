using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Text.Json;

namespace PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel vm)
        {
            if (vm.UserType == 1 && vm.IdentityNumber?.Length != 11)
                return Json(new { success = false, message = "Ger�ek ki�i i�in TCKN 11 haneli olmal�d�r." });

            if (vm.UserType == 2 && vm.IdentityNumber?.Length != 10)
                return Json(new { success = false, message = "T�zel ki�i i�in VKN 10 haneli olmal�d�r." });

            var body = new
            {
                person = new
                {
                    name = vm.Name,
                    identityNumber = long.TryParse(vm.IdentityNumber, out var idNo) ? idNo : 0,
                    taxOffice = vm.TaxOffice ?? string.Empty,
                    // PersonType.User = 0
                    type = 0,
                    status = 1
                },
                status = 1,
                type = vm.UserType,
                email = NormalizeEmail(vm.Email ?? string.Empty),
                password = vm.Password,
                lockoutEnabled = true
            };

            try
            {
                var client = _httpClientFactory.CreateClient("Api");
                var response = await client.PostAsJsonAsync("User", body);

                if (!response.IsSuccessStatusCode)
                {
                    var errText = await response.Content.ReadAsStringAsync();

                    // JSON format�n� ��z�mleme
                    try
                    {
                        var errObj = JsonSerializer.Deserialize<Dictionary<string, object>>(errText);

                        // "errors" varsa oradan mesajlar� �ek
                        if (errObj != null && errObj.ContainsKey("errors"))
                        {
                            var errors = (JsonElement)errObj["errors"];
                            var messages = new List<string>();

                            foreach (var prop in errors.EnumerateObject())
                            {
                                foreach (var msg in prop.Value.EnumerateArray())
                                {
                                    messages.Add(msg.GetString());
                                }
                            }

                            return Json(new { success = false, message = string.Join(" | ", messages) });
                        }
                        if (errObj != null && errObj.ContainsKey("message"))
                        {
                            return Json(new { success = false, message = (JsonElement)errObj["message"] });
                        }
                    }
                    catch
                    {
                        // JSON parse edilemezse oldu�u gibi d�ner
                        return Json(new { success = false, message = errText });
                    }

                    return Json(new { success = false, message = "Kay�t ba�ar�s�z." });
                }

                return Json(new { success = true, message = "Kayıt başarılı! Giriş yapabilirsiniz." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Sunucu hatas�: {ex.Message}" });
            }
        }

        static string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return string.Empty;
            var trimmed = email.Trim();
            var formD = trimmed.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder(formD.Length);
            foreach (var ch in formD)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC).ToLowerInvariant();
        }
    }
}