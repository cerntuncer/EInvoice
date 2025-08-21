using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using PresentationLayer.Models.ApiResponses;
using PresentationLayer.Models.ApiRequests;

namespace EInvoice.Controllers
{
    [Authorize]
    public class CurrentsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CurrentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Kullanıcı kimliğini al
            var meResponse = await client.GetAsync("/Credential/Me");
            if (!meResponse.IsSuccessStatusCode)
                return Unauthorized(new { message = "Kullanıcı doğrulanamadı." });

            var me = await meResponse.Content.ReadFromJsonAsync<GetMyCredentialResponse>();
            if (me == null || me.Error)
                return Unauthorized(new { message = me?.Message ?? "Kullanıcı doğrulanamadı." });

            var resp = await client.GetAsync($"/Current/ByUser/{me.UserId}");
            var json = await resp.Content.ReadAsStringAsync();
            return StatusCode((int)resp.StatusCode, json);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBank([FromBody] CreateBankProxyRequest model)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Eğer yeni cari oluşturulacaksa Current bilgisine UserId ekle
            if (model.Current == null && model.CurrentId == null)
                return BadRequest(new { message = "CurrentId ya da Current bilgisi gerekli." });

            if (model.Current != null)
            {
                var meResponse = await client.GetAsync("/Credential/Me");
                if (!meResponse.IsSuccessStatusCode)
                    return Unauthorized(new { message = "Kullanıcı doğrulanamadı." });
                var me = await meResponse.Content.ReadFromJsonAsync<GetMyCredentialResponse>();
                if (me == null || me.Error)
                    return Unauthorized(new { message = me?.Message ?? "Kullanıcı doğrulanamadı." });
                model.Current.UserId = me.UserId;
            }

            var apiRes = await client.PostAsJsonAsync("/Bank", model);
            var body = await apiRes.Content.ReadAsStringAsync();
            return StatusCode((int)apiRes.StatusCode, body);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCase([FromBody] CreateCaseProxyRequest model)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (model.Current == null && model.CurrentId == null)
                return BadRequest(new { message = "CurrentId ya da Current bilgisi gerekli." });

            if (model.Current != null)
            {
                var meResponse = await client.GetAsync("/Credential/Me");
                if (!meResponse.IsSuccessStatusCode)
                    return Unauthorized(new { message = "Kullanıcı doğrulanamadı." });
                var me = await meResponse.Content.ReadFromJsonAsync<GetMyCredentialResponse>();
                if (me == null || me.Error)
                    return Unauthorized(new { message = me?.Message ?? "Kullanıcı doğrulanamadı." });
                model.Current.UserId = me.UserId;
            }

            var apiRes = await client.PostAsJsonAsync("/Case", model);
            var body = await apiRes.Content.ReadAsStringAsync();
            return StatusCode((int)apiRes.StatusCode, body);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBank([FromBody] UpdateBankProxyRequest model)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiRes = await client.PutAsJsonAsync("/Bank", model);
            var body = await apiRes.Content.ReadAsStringAsync();
            return StatusCode((int)apiRes.StatusCode, body);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCase([FromBody] UpdateCaseProxyRequest model)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiRes = await client.PutAsJsonAsync("/Case", model);
            var body = await apiRes.Content.ReadAsStringAsync();
            return StatusCode((int)apiRes.StatusCode, body);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCurrent(long id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var meResponse = await client.GetAsync("/Credential/Me");
            if (!meResponse.IsSuccessStatusCode)
                return Unauthorized(new { message = "Kullanıcı doğrulanamadı." });
            var me = await meResponse.Content.ReadFromJsonAsync<GetMyCredentialResponse>();
            if (me == null || me.Error)
                return Unauthorized(new { message = me?.Message ?? "Kullanıcı doğrulanamadı." });

            var apiRes = await client.DeleteAsync($"/Current/{id}?userId={me.UserId}");
            var body = await apiRes.Content.ReadAsStringAsync();
            return StatusCode((int)apiRes.StatusCode, body);
        }
    }
}
