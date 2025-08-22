using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ApiResponses;
using System.Net.Http.Headers;

namespace EInvoice.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InvoicesController(IHttpClientFactory httpClientFactory)
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

            var resp = await client.GetAsync($"/Invoice/ByUser/{me.UserId}");
            if (!resp.IsSuccessStatusCode)
            {
                return Ok(new { Items = Array.Empty<object>() });
            }
            var json = await resp.Content.ReadAsStringAsync();
            return new ContentResult { Content = json, ContentType = "application/json", StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiRes = await client.GetAsync($"/Invoice/{id}");
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }

        [HttpGet]
        public async Task<IActionResult> GetFull(long id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // 1) Invoice
            var invHttp = await client.GetAsync($"/Invoice/{id}");
            if (!invHttp.IsSuccessStatusCode)
                return new ContentResult { Content = await invHttp.Content.ReadAsStringAsync(), ContentType = "application/json", StatusCode = (int)invHttp.StatusCode };
            var invoice = await invHttp.Content.ReadFromJsonAsync<InvoiceDto>();
            if (invoice == null) return NotFound(new { message = "Fatura bulunamadı" });

            // 2) Current -> User
            var currentHttp = await client.GetAsync($"/Current/{invoice.CurrentId}");
            CurrentDto? current = null;
            if (currentHttp.IsSuccessStatusCode)
                current = await currentHttp.Content.ReadFromJsonAsync<CurrentDto>();

            UserWithPersonDto? userWithPerson = null;
            if (current?.UserId > 0)
            {
                var userHttp = await client.GetAsync($"/User/WithPerson/{current.UserId}");
                if (userHttp.IsSuccessStatusCode)
                    userWithPerson = await userHttp.Content.ReadFromJsonAsync<UserWithPersonDto>();
            }

            // 3) Customer/Supplier -> Person
            PersonDto? csPerson = null;
            if (invoice.CustomerSupplierId > 0)
            {
                var csHttp = await client.GetAsync($"/CustomerSupplier/{invoice.CustomerSupplierId}");
                if (csHttp.IsSuccessStatusCode)
                {
                    var cs = await csHttp.Content.ReadFromJsonAsync<CustomerSupplierDto>();
                    if (cs != null && cs.PersonId > 0)
                    {
                        var pHttp = await client.GetAsync($"/Person/{cs.PersonId}");
                        if (pHttp.IsSuccessStatusCode)
                            csPerson = await pHttp.Content.ReadFromJsonAsync<PersonDto>();
                    }
                }
            }

            // 4) Addresses for both persons
            var userPersonAddresses = new List<AddressItemDto>();
            var csPersonAddresses = new List<AddressItemDto>();
            if (userWithPerson?.PersonId > 0)
            {
                var adrHttp = await client.GetAsync($"/Address/ByPerson/{userWithPerson.PersonId}");
                if (adrHttp.IsSuccessStatusCode)
                {
                    var ar = await adrHttp.Content.ReadFromJsonAsync<GetAddressesByPersonIdResponseDto>();
                    if (ar?.Addresses != null) userPersonAddresses = ar.Addresses;
                }
            }
            if (csPerson?.Id > 0)
            {
                var adrHttp = await client.GetAsync($"/Address/ByPerson/{csPerson.Id}");
                if (adrHttp.IsSuccessStatusCode)
                {
                    var ar = await adrHttp.Content.ReadFromJsonAsync<GetAddressesByPersonIdResponseDto>();
                    if (ar?.Addresses != null) csPersonAddresses = ar.Addresses;
                }
            }

            // 5) Product details for lines
            var productIdSet = (invoice.Lines ?? new List<LineDto>()).Select(l => l.ProductAndServiceId).Distinct().ToList();
            var productMap = new Dictionary<long, ProductDto>();
            foreach (var pid in productIdSet)
            {
                var pHttp = await client.GetAsync($"/ProductAndService/{pid}");
                if (pHttp.IsSuccessStatusCode)
                {
                    var p = await pHttp.Content.ReadFromJsonAsync<ProductDto>();
                    if (p != null) productMap[pid] = p;
                }
            }
            var lineDetails = (invoice.Lines ?? new List<LineDto>()).Select(l => new LineDetailDto
            {
                Id = l.Id,
                InvoiceId = l.InvoiceId,
                ProductAndServiceId = l.ProductAndServiceId,
                ProductName = productMap.TryGetValue(l.ProductAndServiceId, out var pd) ? pd.Name : string.Empty,
                UnitType = productMap.TryGetValue(l.ProductAndServiceId, out var pd2) ? pd2.UnitType : 0,
                Quantity = l.Quantity,
                UnitPrice = l.UnitPrice,
                VatRate = l.VatRate
            }).ToList();

            var userPhones = userPersonAddresses.Where(a => a.AddressType == 4 && a.Status == 1).Select(a => a.Text).ToList();
            var userBranches = userPersonAddresses.Where(a => a.AddressType == 2 && a.Status == 1).Select(a => a.Text).ToList();
            var userEmails = userPersonAddresses.Where(a => a.AddressType == 1 && a.Status == 1).Select(a => a.Text).ToList();
            var userWebsites = userPersonAddresses.Where(a => a.AddressType == 5 && a.Status == 1).Select(a => a.Text).ToList();
            var csPhones = csPersonAddresses.Where(a => a.AddressType == 4 && a.Status == 1).Select(a => a.Text).ToList();
            var csBranches = csPersonAddresses.Where(a => a.AddressType == 2 && a.Status == 1).Select(a => a.Text).ToList();
            var csEmails = csPersonAddresses.Where(a => a.AddressType == 1 && a.Status == 1).Select(a => a.Text).ToList();
            var csWebsites = csPersonAddresses.Where(a => a.AddressType == 5 && a.Status == 1).Select(a => a.Text).ToList();

            return Ok(new
            {
                Invoice = invoice,
                Current = current,
                LineDetails = lineDetails,
                CurrentUserPerson = userWithPerson == null ? null : new
                {
                    userWithPerson.PersonId,
                    userWithPerson.PersonName,
                    userWithPerson.IdentityNumber,
                    userWithPerson.TaxOffice,
                    Phones = userPhones,
                    BranchAddresses = userBranches,
                    Emails = userEmails,
                    Websites = userWebsites
                },
                CustomerSupplierPerson = csPerson == null ? null : new
                {
                    PersonId = csPerson.Id,
                    PersonName = csPerson.Name,
                    csPerson.IdentityNumber,
                    csPerson.TaxOffice,
                    Phones = csPhones,
                    BranchAddresses = csBranches,
                    Emails = csEmails,
                    Websites = csWebsites
                }
            });
        }

        private class InvoiceDto
        {
            public long Id { get; set; }
            public int Type { get; set; }
            public int Senario { get; set; }
            public DateTime Date { get; set; }
            public int No { get; set; }
            public long CurrentId { get; set; }
            public long CustomerSupplierId { get; set; }
            public int Status { get; set; }
            public string? Ettn { get; set; }
            public List<LineDto> Lines { get; set; } = new();
        }
        private class LineDto
        {
            public long Id { get; set; }
            public long InvoiceId { get; set; }
            public long ProductAndServiceId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public int VatRate { get; set; }
        }
        private class LineDetailDto
        {
            public long Id { get; set; }
            public long InvoiceId { get; set; }
            public long ProductAndServiceId { get; set; }
            public string ProductName { get; set; }
            public int UnitType { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public int VatRate { get; set; }
        }
        private class CurrentDto
        {
            public long Id { get; set; }
            public string? Name { get; set; }
            public decimal Balance { get; set; }
            public int CurrencyType { get; set; }
            public int CurrentType { get; set; }
            public long UserId { get; set; }
            public int Status { get; set; }
        }
        private class UserWithPersonDto
        {
            public long UserId { get; set; }
            public int UserType { get; set; }
            public int UserStatus { get; set; }
            public long PersonId { get; set; }
            public string PersonName { get; set; }
            public long IdentityNumber { get; set; }
            public string? TaxOffice { get; set; }
            public int PersonType { get; set; }
            public int PersonStatus { get; set; }
        }
        private class CustomerSupplierDto
        {
            public long Id { get; set; }
            public int Type { get; set; }
            public long PersonId { get; set; }
            public int Status { get; set; }
        }
        private class PersonDto
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public long IdentityNumber { get; set; }
            public string? TaxOffice { get; set; }
            public int Type { get; set; }
            public int Status { get; set; }
        }
        private class GetAddressesByPersonIdResponseDto
        {
            public string? Message { get; set; }
            public bool Error { get; set; }
            public List<AddressItemDto> Addresses { get; set; } = new();
        }
        private class AddressItemDto
        {
            public long Id { get; set; }
            public string Text { get; set; }
            public int AddressType { get; set; }
            public int Status { get; set; }
        }
        private class ProductDto
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public decimal UnitPrice { get; set; }
            public int UnitType { get; set; }
            public int Status { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] object model)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiRes = await client.PostAsJsonAsync("/Invoice", model);
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] object model)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiRes = await client.PutAsJsonAsync("/Invoice", model);
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { message = "Oturum süresi doldu." });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiRes = await client.DeleteAsync($"/Invoice/{id}");
            var body = await apiRes.Content.ReadAsStringAsync();
            return new ContentResult { Content = body, ContentType = "application/json", StatusCode = (int)apiRes.StatusCode };
        }

        [HttpGet]
        public IActionResult Preview(long id)
        {
            ViewData["Title"] = "Fatura Önizleme";
            ViewBag.InvoiceId = id;
            return View();
        }
    }
}