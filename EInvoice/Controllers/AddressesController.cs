using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using PresentationLayer.Models.ApiResponses;
using PresentationLayer.Models.ApiRequests;

namespace EInvoice.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddressesController(IHttpClientFactory httpClientFactory)
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

            var userResponse = await client.GetAsync($"/User/WithPerson/{me.UserId}");
            if (!userResponse.IsSuccessStatusCode)
                return Ok(new { Addresses = Array.Empty<object>() });
            var user = await userResponse.Content.ReadFromJsonAsync<GetUserWithPersonByIdResponse>();
            if (user == null || user.Error)
                return Ok(new { Addresses = Array.Empty<object>() });

            var resp = await client.GetAsync($"/Address/ByPerson/{user.PersonId}");
            if (!resp.IsSuccessStatusCode)
            {
                return Ok(new { Addresses = Array.Empty<object>() });
            }
            var json = await resp.Content.ReadAsStringAsync();
            return new ContentResult { Content = json, ContentType = "application/json", StatusCode = 200 };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateAddressRequest model)
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

            var userResponse = await client.GetAsync($"/User/WithPerson/{me.UserId}");
            if (!userResponse.IsSuccessStatusCode)
                return BadRequest(new { message = "Kullanıcı bilgileri alınamadı." });
            var user = await userResponse.Content.ReadFromJsonAsync<GetUserWithPersonByIdResponse>();
            if (user == null || user.Error)
                return BadRequest(new { message = user?.Message ?? "Kullanıcı bilgileri alınamadı." });

            var payload = new
            {
                AddressType = model.AddressType,
                Text = model.Text,
                PersonId = user.PersonId
            };

            var apiRes = await client.PostAsJsonAsync("/Address", payload);
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] UpdateAddressRequest model)
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

            var userResponse = await client.GetAsync($"/User/WithPerson/{me.UserId}");
            if (!userResponse.IsSuccessStatusCode)
                return BadRequest(new { message = "Kullanıcı bilgileri alınamadı." });
            var user = await userResponse.Content.ReadFromJsonAsync<GetUserWithPersonByIdResponse>();
            if (user == null || user.Error)
                return BadRequest(new { message = user?.Message ?? "Kullanıcı bilgileri alınamadı." });

            var payload = new
            {
                Id = model.Id,
                AddressType = model.AddressType,
                Text = model.Text,
                PersonId = user.PersonId
            };

            var apiRes = await client.PutAsJsonAsync("/Address", payload);
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

            var meResponse = await client.GetAsync("/Credential/Me");
            if (!meResponse.IsSuccessStatusCode)
                return Unauthorized(new { message = "Kullanıcı doğrulanamadı." });
            var me = await meResponse.Content.ReadFromJsonAsync<GetMyCredentialResponse>();
            if (me == null || me.Error)
                return Unauthorized(new { message = me?.Message ?? "Kullanıcı doğrulanamadı." });

            var userResponse = await client.GetAsync($"/User/WithPerson/{me.UserId}");
            if (!userResponse.IsSuccessStatusCode)
                return BadRequest(new { message = "Kullanıcı bilgileri alınamadı." });
            var user = await userResponse.Content.ReadFromJsonAsync<GetUserWithPersonByIdResponse>();
            if (user == null || user.Error)
                return BadRequest(new { message = user?.Message ?? "Kullanıcı bilgileri alınamadı." });

            var apiRes = await client.DeleteAsync($"/Address/{id}?personId={user.PersonId}");
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }
    }
}