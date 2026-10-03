using HomeServiceManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeServiceManagementSystem.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Service)
                .Include(i => i.Booking)
                    .ThenInclude(b => b.User)
                .Include(i => i.Booking)
                    .ThenInclude(b => b.ServiceProvider)
                        .ThenInclude(sp => sp.User)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }
    }
}