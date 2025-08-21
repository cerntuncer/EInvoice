using System.Diagnostics;
using EInvoice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatabaseAccessLayer.Contexts;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using DatabaseAccessLayer.Enumerations;

namespace EInvoice.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly MyContext _context;

        public DashboardController(ILogger<DashboardController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;

                var userId = User.FindFirst("sub")?.Value;        // JWT'deki "sub" alan�
                var email = User.FindFirst("email")?.Value;       // JWT'deki "email" claim'i
                var role = User.FindFirst("role")?.Value;         // Kullan�c�n�n rol�
            }

            var users = await _context.Users
                .Include(u => u.Person)
                .Select(u => new DashboardUserListItemViewModel
                {
                    UserId = u.Id,
                    Name = u.Person.Name,
                    UserType = u.Type == UserType.NaturalPerson ? "Gerçek Kişi" : "Tüzel Kişi"
                })
                .ToListAsync();

            return View(users);
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
