using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.Controllers
{
    [Authorize]
    public class ProductsAndServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

