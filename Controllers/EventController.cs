using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.Controllers;

public class EventController : Controller
{
    private readonly DataBaseContext _context;

    public EventController(DataBaseContext context)
    {
        _context = context;
    }

    [HttpGet, ActionName("Index")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Events.ToListAsync());
    }

    [HttpGet, ActionName("Details")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events
            .FirstOrDefaultAsync(m => m.Id == id);
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    [HttpGet, ActionName("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost, ActionName("Create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,UserId,Name,Description,Date")] Event @event)
    {
        if (!ModelState.IsValid)
        {
            return View(@event);
        }
        
        _context.Add(@event);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

    }

    [HttpGet, ActionName("Edit")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,Description,Date")] Event @event)
    {
        if (id != @event.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(@event);
        }
        
        try
        {
            _context.Update(@event);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EventExists(@event.Id))
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

    [HttpGet, ActionName("Delete")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event != null)
        {
            _context.Events.Remove(@event);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EventExists(int id)
    {
        return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}