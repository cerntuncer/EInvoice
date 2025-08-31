using System.Diagnostics;
using EInvoice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ApiResponses;
using System.Net.Http.Headers;
using PresentationLayer.Models;

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

            // Fetch full name for navbar
            try
            {
                var userResponse = await client.GetAsync($"/User/WithPerson/{me.UserId}");
                if (userResponse.IsSuccessStatusCode)
                {
                    var user = await userResponse.Content.ReadFromJsonAsync<GetUserWithPersonByIdResponse>();
                    if (user != null && !user.Error)
                    {
                        ViewBag.UserFullName = user.PersonName;
                    }
                }
            }
            catch { }

            // Basit model
            var model = new DashboardViewModel();

            // 3) Invoices by user
            try
            {
                var invoicesHttp = await client.GetAsync($"/Invoice/ByUser/{me.UserId}");
                if (invoicesHttp.IsSuccessStatusCode)
                {
                    var invoices = await invoicesHttp.Content.ReadFromJsonAsync<GetInvoicesByUserIdResponse>();
                    if (invoices != null && !invoices.Error && invoices.Items != null)
                    {
                        model.Invoices = invoices.Items;
                        model.TotalPurchaseAmount = invoices.Items.Where(x => x.Type == 1).Sum(x => x.TotalAmount);
                        model.TotalSalesAmount = invoices.Items.Where(x => x.Type == 2).Sum(x => x.TotalAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invoices fetch failed");
            }

            // 4) Currents by user
            try
            {
                var currentsHttp = await client.GetAsync($"/Current/ByUser/{me.UserId}");
                if (currentsHttp.IsSuccessStatusCode)
                {
                    var currents = await currentsHttp.Content.ReadFromJsonAsync<GetCurrentsByUserIdResponse>();
                    if (currents != null && !currents.Error && currents.Currents != null)
                    {
                        model.Currents = currents.Currents;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Currents fetch failed");
            }

            return View(model);
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