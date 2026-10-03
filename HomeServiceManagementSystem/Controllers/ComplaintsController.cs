using HomeServiceManagementSystem.Data;
using HomeServiceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeServiceManagementSystem.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComplaintsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Complaints
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user == null || user.Role != "Admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var complaints = await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.Booking)
                    .ThenInclude(b => b.Service)
                .Include(c => c.Booking)
                    .ThenInclude(b => b.ServiceProvider)
                        .ThenInclude(sp => sp.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(complaints);
        }

        // GET: Complaints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.Booking)
                    .ThenInclude(b => b.Service)
                .Include(c => c.Booking)
                    .ThenInclude(b => b.ServiceProvider)
                        .ThenInclude(sp => sp.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        // GET: Complaints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.Booking)
                    .ThenInclude(b => b.Service)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        // POST: Complaints/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            string status)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound();
            }

            complaint.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = complaint.Id });
        }

        // GET: Complaints/Create
        public async Task<IActionResult> Create(int bookingId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var booking = await _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.ServiceProvider)
                    .ThenInclude(sp => sp.User)
                .FirstOrDefaultAsync(b =>
                    b.Id == bookingId &&
                    b.UserId == userId.Value);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Complaints/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int bookingId,
            string subject,
            string description)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b =>
                    b.Id == bookingId &&
                    b.UserId == userId.Value);

            if (booking == null)
            {
                return NotFound();
            }

            var complaint = new Complaint
            {
                UserId = userId.Value,
                BookingId = bookingId,
                Subject = subject,
                Description = description,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Complaints.Add(complaint);

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Bookings", new { id = bookingId });

        }
    }
}