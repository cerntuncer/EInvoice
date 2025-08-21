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
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;

                var userId = User.FindFirst("sub")?.Value;        // JWT'deki "sub" alan�
                var email = User.FindFirst("email")?.Value;       // JWT'deki "email" claim'i
                var role = User.FindFirst("role")?.Value;         // Kullan�c�n�n rol�
            }

            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var apiResponse = await client.GetAsync("/User/WithPerson");
            var users = new List<DashboardUserListItemViewModel>();
            if (apiResponse.IsSuccessStatusCode)
            {
                var data = await apiResponse.Content.ReadFromJsonAsync<GetUsersWithPersonListResponse>();
                if (data != null && !data.Error)
                {
                    users = data.Users
                        .Select(u => new DashboardUserListItemViewModel
                        {
                            UserId = u.UserId,
                            Name = u.PersonName,
                            UserType = u.UserType == 1 ? "Gerçek Kişi" : "Tüzel Kişi",
                            IdentityNumber = u.PersonIdentityNumber,
                            TaxOffice = u.PersonTaxOffice
                        })
                        .ToList();
                }
            }

            return View(users);
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