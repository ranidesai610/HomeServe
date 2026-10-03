using HomeServiceManagementSystem.Data;
using HomeServiceManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeServiceManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users
                .FirstOrDefault(u =>
                    u.Email == model.Email &&
                    u.Password == model.Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View(model);
            }

            if (user.Role == "Customer")
            {
                return RedirectToAction("Index", "Customer");
            }

            if (user.Role == "Provider")
            {
                return RedirectToAction("Index", "Provider");
            }

            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Error = "Invalid user role.";
            return View(model);
        }
    }
}