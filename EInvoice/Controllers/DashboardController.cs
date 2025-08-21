using System.Diagnostics;
using EInvoice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ApiResponses;
using System.Net.Http.Headers;
using PresentationLayer.Models;
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

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(DashboardProfileViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { success = false, message = "Geçersiz form verisi." });
			}

			var client = _httpClientFactory.CreateClient("Api");
			var accessToken = HttpContext.Session.GetString("AccessToken");
			if (string.IsNullOrEmpty(accessToken))
			{
				return Unauthorized(new { success = false, message = "Oturum süresi doldu." });
			}
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			// Güncel değerleri al (kimlik no, tip, durumlar değiştirilmesin)
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

			// Sadece ad ve vergi dairesi güncellenecek; kimlik no, tip ve durumlar korunur
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

			// Kullanıcı tipi/durumu güncellenmez

			return Ok(new { success = true, message = "Profil başarıyla güncellendi." });
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