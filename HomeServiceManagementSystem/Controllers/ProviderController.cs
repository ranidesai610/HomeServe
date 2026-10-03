using HomeServiceManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeServiceManagementSystem.Controllers
{
    public class ProviderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProviderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? providerUserId = HttpContext.Session.GetInt32("UserId");

            if (providerUserId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var provider = _context.ServiceProviders
                .FirstOrDefault(sp => sp.UserId == providerUserId.Value);

            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        public async Task<IActionResult> Bookings()
        {
            int? providerUserId = HttpContext.Session.GetInt32("UserId");

            if (providerUserId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var provider = await _context.ServiceProviders
                .FirstOrDefaultAsync(sp => sp.UserId == providerUserId.Value);

            if (provider == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings
                .Where(b => b.ServiceProviderId == provider.Id)
                .Include(b => b.User)
                .Include(b => b.Service)
                .Include(b => b.Address)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        // Accept booking
        [HttpPost]
        public async Task<IActionResult> AcceptBooking(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Confirmed";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Bookings));
        }

        // Reject booking
        [HttpPost]
        public async Task<IActionResult> RejectBooking(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Rejected";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Bookings));
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> Profile()
        {
            int? providerUserId = HttpContext.Session.GetInt32("UserId");

            if (providerUserId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var provider = await _context.ServiceProviders
                .Include(sp => sp.User)
                .Include(sp => sp.ProviderServices)
                    .ThenInclude(ps => ps.Service)
                .FirstOrDefaultAsync(sp => sp.UserId == providerUserId.Value);

            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        [HttpPost]
        public async Task<IActionResult> StartBooking(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "In Progress";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Bookings));
        }

        [HttpPost]
        public async Task<IActionResult> CompleteBooking(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Completed";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Bookings));
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Bookings));
        }
    }
}