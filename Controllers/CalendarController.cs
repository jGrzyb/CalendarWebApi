using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.Controllers;

[Authorize]
public class CalendarController : Controller {
    private readonly DataBaseContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CalendarController(DataBaseContext context, UserManager<IdentityUser> userManager) {
        _context = context;
        _userManager = userManager;
    }

    // GET: Calendar
    public async Task<IActionResult> Index() {
        var calendars = await _context.Ownership
            .Where(o => o.UserId.ToString().ToLower() == (_userManager.GetUserId(User) ?? "").ToLower())
            .Join(_context.Calendar,
                o => o.CalendarId,
                c => c.Id,
                (o, c) => c)
            .ToListAsync();
        return View(calendars);
    }

    // GET: Calendar/Details/5
    public async Task<IActionResult> Details(int? id) {
        if (id == null) {
            return NotFound();
        }

        var calendar = await _context.Calendar
            .FirstOrDefaultAsync(m => m.Id == id);
        if (calendar == null) {
            return NotFound();
        }

        return View(calendar);
    }

    // GET: Calendar/Create
    public IActionResult Create() {
        return View();
    }

    // POST: Calendar/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,IsPublic")] Calendar calendar) {
        if (ModelState.IsValid) {
            _context.Add(calendar);
            await _context.SaveChangesAsync();
            _context.Ownership.Add(new Ownership {
                CalendarId = calendar.Id,
                UserId = Guid.Parse(_userManager.GetUserId(User) ?? ""),
                IsOwner = true
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(calendar);
    }

    // GET: Calendar/Edit/5
    public async Task<IActionResult> Edit(int? id) {
        if (id == null) {
            return NotFound();
        }

        var calendar = await _context.Calendar.FindAsync(id);
        if (calendar == null) {
            return NotFound();
        }

        return View(calendar);
    }

    // POST: Calendar/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsPublic")] Calendar calendar) {
        if (id != calendar.Id) {
            return NotFound();
        }

        if (ModelState.IsValid) {
            try {
                _context.Update(calendar);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!CalendarExists(calendar.Id)) {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(calendar);
    }

    // GET: Calendar/Delete/5
    public async Task<IActionResult> Delete(int? id) {
        if (id == null) {
            return NotFound();
        }

        var calendar = await _context.Calendar
            .FirstOrDefaultAsync(m => m.Id == id);
        if (calendar == null) {
            return NotFound();
        }

        return View(calendar);
    }

    // POST: Calendar/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        var ownerships = await _context.Ownership
            .Where(o => o.UserId.ToString().ToLower() == (_userManager.GetUserId(User) ?? "").ToLower())
            .Where(o => o.CalendarId == id)
            .ToListAsync();
        _context.Ownership.RemoveRange(ownerships);
        await _context.SaveChangesAsync();


        var number = _context.Ownership
            .Count(o => o.CalendarId == id);
        
        var calendar = await _context.Calendar.FindAsync(id);
        if (number == 0 && calendar != null) {
            _context.Calendar.Remove(calendar);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CalendarExists(int id) {
        return _context.Calendar.Any(e => e.Id == id);
    }
}