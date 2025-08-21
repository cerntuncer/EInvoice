using EInvoice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using PresentationLayer.Models.ApiRequests;
using PresentationLayer.Models.ApiResponses;
using System.Net.Http.Headers;

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
				return BadRequest(new { success = false, message = "Geçersiz form verisi." });
			}

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
				IdentityNumber = model.IdentityNumber,
				TaxOffice = model.TaxOffice ?? string.Empty,
				Type = current.PersonType,
				Status = current.PersonStatus
			};

			var personResponse = await client.PutAsJsonAsync("/Person", personUpdateRequest);
			if (!personResponse.IsSuccessStatusCode)
			{
				return BadRequest(new { success = false, message = "Kişi bilgileri güncellenemedi." });
			}

			return Ok(new { success = true, message = "Bilgileriniz başarıyla güncellendi." });
		}
	}
}
