using HomeServiceManagementSystem.Data;
using HomeServiceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeServiceManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var admin = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (admin == null || admin.Role != "Admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var users = await _context.Users
                .OrderBy(u => u.Role)
                .ThenBy(u => u.Name)
                .ToListAsync();

            return View(users);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var admin = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (admin == null || admin.Role != "Admin")
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var admin = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (admin == null || admin.Role != "Admin")
            {
                return RedirectToAction("Index", "Login");
            }

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                ModelState.AddModelError(
                    "Email",
                    "A user with this email already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }
    }
}