using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ApiResponses;
using System.Net.Http.Headers;

namespace EInvoice.Controllers
{
    [Authorize]
    public class ProductsAndServicesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsAndServicesController(IHttpClientFactory httpClientFactory)
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

            var meResponse = await client.GetAsync("/Credential/Me");
            if (!meResponse.IsSuccessStatusCode)
                return Unauthorized(new { message = "Kullanıcı doğrulanamadı." });

            var me = await meResponse.Content.ReadFromJsonAsync<GetMyCredentialResponse>();
            if (me == null || me.Error)
                return Unauthorized(new { message = me?.Message ?? "Kullanıcı doğrulanamadı." });

            var resp = await client.GetAsync($"/ProductAndService/ByUser/{me.UserId}");
            if (!resp.IsSuccessStatusCode)
            {
                return Ok(new { Items = Array.Empty<object>() });
            }
            var json = await resp.Content.ReadAsStringAsync();
            return new ContentResult { Content = json, ContentType = "application/json", StatusCode = 200 };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] object model)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiRes = await client.PostAsJsonAsync("/ProductAndService", model);
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] object model)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiRes = await client.PutAsJsonAsync("/ProductAndService", model);
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiRes = await client.DeleteAsync($"/ProductAndService/{id}");
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }
    }
}
