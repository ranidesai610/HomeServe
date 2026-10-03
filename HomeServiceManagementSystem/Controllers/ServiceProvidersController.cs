using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeServiceManagementSystem.Models;
using HomeServiceManagementSystem.Data;
using ServiceProviderModel = HomeServiceManagementSystem.Models.ServiceProvider;

public class ServiceProvidersController : Controller
{
    private readonly ApplicationDbContext _context;

    public ServiceProvidersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ServiceProviders
    public async Task<IActionResult> Index()
    {
        var serviceProviders = await _context.ServiceProviders
            .Include(s => s.User)
            .Include(s => s.ProviderServices)
                .ThenInclude(ps => ps.Service)
            .ToListAsync();

        return View(serviceProviders);
    }

    // GET: ServiceProviders/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var serviceProvider = await _context.ServiceProviders
            .Include(s => s.User)
            .Include(s => s.ProviderServices)
                .ThenInclude(ps => ps.Service)
            .Include(s => s.ProviderAvailabilities)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (serviceProvider == null)
        {
            return NotFound();
        }

        return View(serviceProvider);
    }

    // GET: ServiceProviders/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Users = await _context.Users
            .Where(u => u.Role == "Provider")
            .ToListAsync();

        ViewBag.Services = await _context.Services
            .ToListAsync();

        return View();
    }

    // POST: ServiceProviders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ServiceProviderModel serviceProvider,
        int[] selectedServices)
    {
        if (ModelState.IsValid)
        {
            _context.ServiceProviders.Add(serviceProvider);
            await _context.SaveChangesAsync();

            if (selectedServices != null)
            {
                foreach (var serviceId in selectedServices)
                {
                    var providerService = new ProviderService
                    {
                        ServiceProviderId = serviceProvider.Id,
                        ServiceId = serviceId
                    };

                    _context.ProviderServices.Add(providerService);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Users = await _context.Users
            .Where(u => u.Role == "Provider")
            .ToListAsync();

        ViewBag.Services = await _context.Services
            .ToListAsync();

        return View(serviceProvider);
    }

    // GET: ServiceProviders/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var serviceProvider = await _context.ServiceProviders
            .Include(s => s.ProviderServices)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (serviceProvider == null)
        {
            return NotFound();
        }

        ViewBag.Users = await _context.Users
            .Where(u => u.Role == "Provider")
            .ToListAsync();

        ViewBag.Services = await _context.Services
            .ToListAsync();

        ViewBag.SelectedServices = serviceProvider.ProviderServices?
            .Select(ps => ps.ServiceId)
            .ToList();

        return View(serviceProvider);
    }

    // POST: ServiceProviders/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        ServiceProviderModel serviceProvider,
        int[] selectedServices)
    {
        if (id != serviceProvider.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(serviceProvider);
                await _context.SaveChangesAsync();

                // Remove old services
                var oldServices = await _context.ProviderServices
                    .Where(ps => ps.ServiceProviderId == id)
                    .ToListAsync();

                _context.ProviderServices.RemoveRange(oldServices);

                // Add new services
                if (selectedServices != null)
                {
                    foreach (var serviceId in selectedServices)
                    {
                        _context.ProviderServices.Add(
                            new ProviderService
                            {
                                ServiceProviderId = id,
                                ServiceId = serviceId
                            });
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceProviderExists(serviceProvider.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Users = await _context.Users
            .Where(u => u.Role == "Provider")
            .ToListAsync();

        ViewBag.Services = await _context.Services
            .ToListAsync();

        return View(serviceProvider);
    }

    // GET: ServiceProviders/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var serviceProvider = await _context.ServiceProviders
            .Include(s => s.User)
            .Include(s => s.ProviderServices)
                .ThenInclude(ps => ps.Service)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (serviceProvider == null)
        {
            return NotFound();
        }

        return View(serviceProvider);
    }

    // POST: ServiceProviders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var serviceProvider = await _context.ServiceProviders
            .FindAsync(id);

        if (serviceProvider != null)
        {
            _context.ServiceProviders.Remove(serviceProvider);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool ServiceProviderExists(int id)
    {
        return _context.ServiceProviders
            .Any(e => e.Id == id);
    }
}