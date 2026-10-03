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

        var provideravailability = await _context.ProviderAvailabilities
            .Include(p => p.ServiceProvider)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (provideravailability == null)
        {
            return NotFound();
        }

        return View(provideravailability);
    }

    // GET: ProviderAvailabilities/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ProviderAvailabilities/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,ServiceProviderId,AvailableDate,StartTime,EndTime,IsAvailable")]
    ProviderAvailability provideravailability)
    {
        if (ModelState.IsValid)
        {
            _context.Add(provideravailability);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(provideravailability);
    }

    // GET: ProviderAvailabilities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var provideravailability = await _context.ProviderAvailabilities
            .FindAsync(id);

        if (provideravailability == null)
        {
            return NotFound();
        }

        return View(provideravailability);
    }

    // POST: ProviderAvailabilities/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,ServiceProviderId,AvailableDate,StartTime,EndTime,IsAvailable")]
    ProviderAvailability provideravailability)
    {
        if (id != provideravailability.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(provideravailability);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderAvailabilityExists(provideravailability.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(provideravailability);
    }

    // GET: ProviderAvailabilities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var provideravailability = await _context.ProviderAvailabilities
            .Include(p => p.ServiceProvider)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (provideravailability == null)
        {
            return NotFound();
        }

        return View(provideravailability);
    }

    // POST: ProviderAvailabilities/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var provideravailability = await _context.ProviderAvailabilities
            .FindAsync(id);

        if (provideravailability != null)
        {
            _context.ProviderAvailabilities.Remove(provideravailability);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool ProviderAvailabilityExists(int? id)
    {
        return _context.ProviderAvailabilities.Any(e => e.Id == id);
    }

}
