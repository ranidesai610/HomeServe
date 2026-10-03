using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeServiceManagementSystem.Models;
using HomeServiceManagementSystem.Data;

public class ProviderAvailabilitiesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProviderAvailabilitiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ProviderAvailabilities
    public async Task<IActionResult> Index()
    {
        var availabilities = await _context.ProviderAvailabilities
            .Include(p => p.ServiceProvider)
                .ThenInclude(sp => sp.User)
            .ToListAsync();

        return View(availabilities);
    }

    // GET: ProviderAvailabilities/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var providerAvailability = await _context.ProviderAvailabilities
            .Include(p => p.ServiceProvider)
                .ThenInclude(sp => sp.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (providerAvailability == null)
        {
            return NotFound();
        }

        return View(providerAvailability);
    }

    // GET: ProviderAvailabilities/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Providers = await _context.ServiceProviders
            .Include(sp => sp.User)
            .ToListAsync();

        return View();
    }

    // POST: ProviderAvailabilities/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,ServiceProviderId,DayOfWeek,StartTime,EndTime,IsAvailable")]
        ProviderAvailability providerAvailability)
    {
        if (ModelState.IsValid)
        {
            _context.Add(providerAvailability);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Providers = await _context.ServiceProviders
            .Include(sp => sp.User)
            .ToListAsync();

        return View(providerAvailability);
    }

    // GET: ProviderAvailabilities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var providerAvailability = await _context.ProviderAvailabilities
            .FindAsync(id);

        if (providerAvailability == null)
        {
            return NotFound();
        }

        ViewBag.Providers = await _context.ServiceProviders
            .Include(sp => sp.User)
            .ToListAsync();

        return View(providerAvailability);
    }

    // POST: ProviderAvailabilities/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,ServiceProviderId,DayOfWeek,StartTime,EndTime,IsAvailable")]
        ProviderAvailability providerAvailability)
    {
        if (id != providerAvailability.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(providerAvailability);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderAvailabilityExists(providerAvailability.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Providers = await _context.ServiceProviders
            .Include(sp => sp.User)
            .ToListAsync();

        return View(providerAvailability);
    }

    // GET: ProviderAvailabilities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var providerAvailability = await _context.ProviderAvailabilities
            .Include(p => p.ServiceProvider)
                .ThenInclude(sp => sp.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (providerAvailability == null)
        {
            return NotFound();
        }

        return View(providerAvailability);
    }

    // POST: ProviderAvailabilities/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var providerAvailability = await _context.ProviderAvailabilities
            .FindAsync(id);

        if (providerAvailability != null)
        {
            _context.ProviderAvailabilities.Remove(providerAvailability);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool ProviderAvailabilityExists(int id)
    {
        return _context.ProviderAvailabilities
            .Any(e => e.Id == id);
    }
}