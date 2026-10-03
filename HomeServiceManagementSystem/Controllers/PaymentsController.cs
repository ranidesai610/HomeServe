using HomeServiceManagementSystem.Data;
using HomeServiceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeServiceManagementSystem.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payments/Create
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

            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);

            if (existingPayment != null)
            {
                return RedirectToAction(nameof(Details), new { id = existingPayment.Id });
            }

            return View(booking);
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int bookingId,
            string paymentMethod)
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

            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);

            if (existingPayment != null)
            {
                return RedirectToAction(nameof(Details),
                    new { id = existingPayment.Id });
            }

            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = booking.TotalAmount,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Paid",
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            var invoice = new Invoice
            {
                BookingId = booking.Id,
                Amount = booking.TotalAmount,
                InvoiceNumber = "INV-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                InvoiceDate = DateTime.Now,
                PaymentStatus = "Paid"
            };


            _context.Invoices.Add(invoice);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details),
                new { id = payment.Id });
        }
        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Service)
                        .Include(p => p.Booking)
        .ThenInclude(b => b.Invoice)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.User)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.ServiceProvider)
                        .ThenInclude(sp => sp.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }
    }
}