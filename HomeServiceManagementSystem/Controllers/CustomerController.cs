using HomeServiceManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace HomeServiceManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");

            if (customerId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var customer = _context.Users
                .FirstOrDefault(u => u.Id == customerId.Value);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Login");
        }

    }
}