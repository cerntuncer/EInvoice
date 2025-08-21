using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.Controllers
{
    [Authorize]
    public class CurrentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
