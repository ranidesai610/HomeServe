using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeServiceManagementSystem.Models;
using HomeServiceManagementSystem.Data;

namespace HomeServiceManagementSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings

        public async Task<IActionResult> Index()
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");

            if (customerId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var bookings = await _context.Bookings
                .Where(b => b.UserId == customerId.Value)
                .Include(b => b.Service)
                .Include(b => b.ServiceProvider)
                    .ThenInclude(sp => sp.User)
                .Include(b => b.Address)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }
        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.ServiceProvider)
                    .ThenInclude(sp => sp.User)
                .Include(b => b.Address)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create(int serviceId)
        {
            var service = await _context.Services
                .Include(s => s.ServiceCategory)
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int serviceId,
            DateTime bookingDate,
            string bookingTime,
            string? description)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return NotFound();
            }

            // Get the selected day.
            // Example: Monday, Tuesday, Wednesday...
            var selectedDay = bookingDate.DayOfWeek.ToString();

            // Find providers who:
            // 1. Are approved
            // 2. Are available
            // 3. Provide the selected service
            // 4. Work on the selected day
            // 5. Are available at the selected time
            var providers = await _context.ServiceProviders
                .Include(sp => sp.User)
                .Include(sp => sp.ProviderServices)
                .Include(sp => sp.ProviderAvailabilities)
                .Where(sp =>
                    sp.IsApproved &&
                    sp.IsAvailable &&
                    sp.ProviderServices!
                        .Any(ps => ps.ServiceId == serviceId) &&
                    sp.ProviderAvailabilities!
                        .Any(pa =>
                            pa.DayOfWeek == selectedDay &&
                            pa.IsAvailable &&
                            string.Compare(pa.StartTime, bookingTime) <= 0 &&
                            string.Compare(pa.EndTime, bookingTime) >= 0)
                )
                .ToListAsync();

            ViewBag.ServiceId = serviceId;
            ViewBag.BookingDate = bookingDate.ToString("yyyy-MM-dd");
            ViewBag.BookingTime = bookingTime;
            ViewBag.Description = description;

            return View(
                "~/Views/Bookings/AvailableProviders/Index.cshtml",
                providers);
        }

        // GET: Bookings/SelectProvider
        public async Task<IActionResult> SelectProvider(
            int providerId,
            int serviceId,
            string bookingDate,
            string bookingTime,
            string? description)
        {
            var provider = await _context.ServiceProviders
                .Include(sp => sp.User)
                .FirstOrDefaultAsync(sp => sp.Id == providerId);

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (provider == null || service == null)
            {
                return NotFound();
            }

            ViewBag.ServiceId = serviceId;
            ViewBag.ProviderId = providerId;
            ViewBag.BookingDate = bookingDate;
            ViewBag.BookingTime = bookingTime;
            ViewBag.Description = description;

            return View(
                "~/Views/Bookings/SelectProvider/Index.cshtml",
                provider);
        }

        // GET: Bookings/Address
        // GET: Bookings/Address
        public async Task<IActionResult> Address(
            int providerId,
            int serviceId,
            string bookingDate,
            string bookingTime,
            string? description)
        {
            var provider = await _context.ServiceProviders
                .Include(sp => sp.User)
                .FirstOrDefaultAsync(sp => sp.Id == providerId);

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (provider == null || service == null)
            {
                return NotFound();
            }

            ViewBag.ProviderId = providerId;
            ViewBag.ServiceId = serviceId;

            ViewBag.ProviderName = provider.User?.Name;
            ViewBag.ServiceName = service.Name;
            ViewBag.ServicePrice = service.Price;

            ViewBag.BookingDate = bookingDate;
            ViewBag.BookingTime = bookingTime;
            ViewBag.Description = description;

            return View("~/Views/Bookings/Address/Index.cshtml");
        }

        // GET: Bookings/Review
        public async Task<IActionResult> Review(
            int providerId,
            int serviceId,
            string bookingDate,
            string bookingTime,
            string? description,
            string houseNumber,
            string street,
            string area,
            string city,
            string state,
            string pincode)
        {
            var provider = await _context.ServiceProviders
                .Include(sp => sp.User)
                .FirstOrDefaultAsync(sp => sp.Id == providerId);

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (provider == null || service == null)
            {
                return NotFound();
            }

            ViewBag.ProviderId = providerId;
            ViewBag.ServiceId = serviceId;

            ViewBag.ProviderName = provider.User?.Name;
            ViewBag.ServiceName = service.Name;
            ViewBag.ServicePrice = service.Price;

            ViewBag.BookingDate = bookingDate;
            ViewBag.BookingTime = bookingTime;
            ViewBag.Description = description;

            ViewBag.HouseNumber = houseNumber;
            ViewBag.Street = street;
            ViewBag.Area = area;
            ViewBag.City = city;
            ViewBag.State = state;
            ViewBag.Pincode = pincode;
          

            return View("~/Views/Bookings/Confirm/Index.cshtml");
        }

        // POST: Bookings/Confirm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(
            int providerId,
            int serviceId,
            string bookingDate,
            string bookingTime,
            string? description,
            string houseNumber,
            string street,
            string area,
            string city,
            string state,
            string pincode)
        {
            var provider = await _context.ServiceProviders
                .Include(sp => sp.User)
                .FirstOrDefaultAsync(sp => sp.Id == providerId);

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (provider == null || service == null)
            {
                return NotFound();
            }

            // Get the currently logged-in customer.
            int? customerId = HttpContext.Session.GetInt32("UserId");

            // If the customer is not logged in,
            // send them to the login page.
            if (customerId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Create the customer's address.
            var address = new Address
            {
                UserId = customerId.Value,
                HouseNumber = houseNumber,
                Street = street,
                Area = area,
                City = city,
                State = state,
                Pincode = pincode
            };

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            // Create the booking.
            var booking = new Booking
            {
                UserId = customerId.Value,
                ServiceProviderId = providerId,
                ServiceId = serviceId,
                AddressId = address.Id,
                BookingDate = DateTime.Parse(bookingDate),
                BookingTime = bookingTime,
                Description = description,
                Status = "Pending",
                TotalAmount = service.Price
            };

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new { id = booking.Id });
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Service)
                .Include(b => b.ServiceProvider)
                    .ThenInclude(sp => sp.User)
                .Include(b => b.Address)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}