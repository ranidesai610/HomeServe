
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeServiceManagementSystem.Models;
using HomeServiceManagementSystem.Data;

public class ServiceCategoriesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ServiceCategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: SERVICECATEGORYS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ServiceCategories.ToListAsync());
    }

    // GET: SERVICECATEGORYS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicecategory = await _context.ServiceCategories
            .FirstOrDefaultAsync(m => m.Id == id);
        if (servicecategory == null)
        {
            return NotFound();
        }

        return View(servicecategory);
    }

    // GET: SERVICECATEGORYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SERVICECATEGORYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] ServiceCategory servicecategory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(servicecategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(servicecategory);
    }

    // GET: SERVICECATEGORYS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicecategory = await _context.ServiceCategories.FindAsync(id);
        if (servicecategory == null)
        {
            return NotFound();
        }
        return View(servicecategory);
    }

    // POST: SERVICECATEGORYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Description")] ServiceCategory servicecategory)
    {
        if (id != servicecategory.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(servicecategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceCategoryExists(servicecategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(servicecategory);
    }

    // GET: SERVICECATEGORYS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicecategory = await _context.ServiceCategories
            .FirstOrDefaultAsync(m => m.Id == id);
        if (servicecategory == null)
        {
            return NotFound();
        }

        return View(servicecategory);
    }

    // POST: SERVICECATEGORYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var servicecategory = await _context.ServiceCategories.FindAsync(id);
        if (servicecategory != null)
        {
            _context.ServiceCategories.Remove(servicecategory);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ServiceCategoryExists(int? id)
    {
        return _context.ServiceCategories.Any(e => e.Id == id);
    }
}
