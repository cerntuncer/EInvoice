using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

