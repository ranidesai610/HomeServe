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

    // View my availability
    public async Task<IActionResult> Index()
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
        var availabilities = await _context.ProviderAvailabilities
            .Where(pa => pa.ServiceProviderId == provider.Id)
            .Include(pa => pa.ServiceProvider)
                .ThenInclude(sp => sp.User)
            .OrderBy(pa => pa.DayOfWeek)
            .ToListAsync();
        return View(availabilities);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

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

        var providerAvailability = await _context.ProviderAvailabilities
            .Include(pa => pa.ServiceProvider)
                .ThenInclude(sp => sp.User)
            .FirstOrDefaultAsync(pa =>
                pa.Id == id &&
                pa.ServiceProviderId == provider.Id);

        if (providerAvailability == null)
        {
            return NotFound();
        }

        return View(providerAvailability);
    }

    // Add availability
    public IActionResult Create()
    {
        int? providerUserId = HttpContext.Session.GetInt32("UserId");

        if (providerUserId == null)
        {
            return RedirectToAction("Index", "Login");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,DayOfWeek,StartTime,EndTime,IsAvailable")]
        ProviderAvailability providerAvailability)
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

        if (ModelState.IsValid)
        {
            providerAvailability.ServiceProviderId = provider.Id;

            _context.ProviderAvailabilities.Add(providerAvailability);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(providerAvailability);
    }

    // Edit availability
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

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

        var providerAvailability = await _context.ProviderAvailabilities
            .FirstOrDefaultAsync(pa =>
                pa.Id == id &&
                pa.ServiceProviderId == provider.Id);

        if (providerAvailability == null)
        {
            return NotFound();
        }

        return View(providerAvailability);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,DayOfWeek,StartTime,EndTime,IsAvailable")]
        ProviderAvailability providerAvailability)
    {
        if (id != providerAvailability.Id)
        {
            return NotFound();
        }

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

        var existingAvailability = await _context.ProviderAvailabilities
            .FirstOrDefaultAsync(pa =>
                pa.Id == id &&
                pa.ServiceProviderId == provider.Id);

        if (existingAvailability == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            existingAvailability.DayOfWeek = providerAvailability.DayOfWeek;
            existingAvailability.StartTime = providerAvailability.StartTime;
            existingAvailability.EndTime = providerAvailability.EndTime;
            existingAvailability.IsAvailable = providerAvailability.IsAvailable;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(providerAvailability);
    }

    // Delete availability
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

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

        var providerAvailability = await _context.ProviderAvailabilities
            .FirstOrDefaultAsync(pa =>
                pa.Id == id &&
                pa.ServiceProviderId == provider.Id);

        if (providerAvailability == null)
        {
            return NotFound();
        }

        return View(providerAvailability);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

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

        var providerAvailability = await _context.ProviderAvailabilities
            .FirstOrDefaultAsync(pa =>
                pa.Id == id &&
                pa.ServiceProviderId == provider.Id);

        if (providerAvailability != null)
        {
            _context.ProviderAvailabilities.Remove(providerAvailability);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}