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
                return Json(new { success = false, message = "Gerçek kiþi için TCKN 11 haneli olmalýdýr." });

            if (vm.UserType == 2 && vm.IdentityNumber?.Length != 10)
                return Json(new { success = false, message = "Tüzel kiþi için VKN 10 haneli olmalýdýr." });

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
                email = vm.Email,
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

                    // JSON formatýný çözümleme
                    try
                    {
                        var errObj = JsonSerializer.Deserialize<Dictionary<string, object>>(errText);

                        // "errors" varsa oradan mesajlarý çek
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
                        // JSON parse edilemezse olduðu gibi döner
                        return Json(new { success = false, message = errText });
                    }

                    return Json(new { success = false, message = "Kayýt baþarýsýz." });
                }

                return Json(new { success = true, message = "Kayýt baþarýlý! Giriþ yapabilirsiniz." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Sunucu hatasý: {ex.Message}" });
            }
        }
    }
}