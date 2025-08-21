using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using PresentationLayer.Models.ApiResponses;

namespace EInvoice.Controllers
{
    [Authorize]
    public class CustomersSuppliersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomersSuppliersController(IHttpClientFactory httpClientFactory)
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

            var resp = await client.GetAsync($"/CustomerSupplier/List");
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

            var apiRes = await client.PostAsJsonAsync("/CustomerSupplier", model);
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

            var apiRes = await client.PutAsJsonAsync("/CustomerSupplier/Update", model);
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

            var apiRes = await client.DeleteAsync($"/CustomerSupplier/Delete/{id}");
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }
    }
}