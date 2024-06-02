using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projekt.Data;
using projekt.Models;

namespace projekt.Controllers;

[Authorize]
public class EventController : Controller {
    private readonly DataBaseContext _context;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public EventController(DataBaseContext context, UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: Event
    public async Task<IActionResult> Index() {
        var user = await _userManager.GetUserAsync(User);
        if (user != null) {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin")) {
                return View(await _context.Event.ToListAsync());
            }
        }

        // else if not admin
        var str = _userManager.GetUserId(User);
        Console.WriteLine(str);
        var events = await _context.Ownership
            .Where(o => o.UserId.ToString().ToLower() == (_userManager.GetUserId(User) ?? "").ToLower())
            .Join(_context.Event,
                o => o.CalendarId,
                e => e.CalendarId,
                (o, e) => e)
            .ToListAsync();
        return View(events);
    }

    // GET: Event/Details/5
    public async Task<IActionResult> Details(int? id) {
        if (id == null) {
            return NotFound();
        }

        var @event = await _context.Event
            .FirstOrDefaultAsync(m => m.Id == id);
        if (@event == null) {
            return NotFound();
        }

        return View(@event);
    }

    // GET: Event/Create
    public async Task<IActionResult> Create() {
        var yourCalendars = await _context.Ownership
            .Where(x => x.UserId.ToString().ToLower() == _userManager.GetUserId(User))
            .ToListAsync();
        ViewBag.Calendars = new SelectList(yourCalendars.Select(x => x.CalendarId).ToList());
        ViewData["Error"] = "You do not own any calendar.";
        return View();
    }

    // POST: Event/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CalendarId,Name,Description,Date")] Event @event) {
        if (ModelState.IsValid) {
            _context.Add(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(@event);
    }

    // GET: Event/Edit/5
    public async Task<IActionResult> Edit(int? id) {
        if (id == null) {
            return NotFound();
        }

        var @event = await _context.Event.FindAsync(id);
        if (@event == null) {
            return NotFound();
        }

        return View(@event);
    }

    // POST: Event/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CalendarId,Name,Description,Date")] Event @event) {
        if (id != @event.Id) {
            return NotFound();
        }

        if (ModelState.IsValid) {
            try {
                _context.Update(@event);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!EventExists(@event.Id)) {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(@event);
    }

    // GET: Event/Delete/5
    public async Task<IActionResult> Delete(int? id) {
        if (id == null) {
            return NotFound();
        }

        var @event = await _context.Event
            .FirstOrDefaultAsync(m => m.Id == id);
        if (@event == null) {
            return NotFound();
        }

        return View(@event);
    }

    // POST: Event/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        var @event = await _context.Event.FindAsync(id);
        if (@event != null) {
            _context.Event.Remove(@event);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EventExists(int id) {
        return _context.Event.Any(e => e.Id == id);
    }
    
    // public async Task<IActionResult> ShowUsers() {
    //     var users = await _userManager.Users
    //         .Select(u => new User { Id = Guid.Parse(u.Id), UserName = u.UserName ?? "NaN" })
    //         .ToListAsync();
    //     return View(users);
    // }
    //
    // public async Task<IActionResult> SendCalendar(Guid? id) {
    //     if (id == null) {
    //         return NotFound();
    //     }
    //
    //     ViewBag.Calendars = new SelectList(await _context.Ownership
    //         .Where(o => o.UserId.ToString().ToLower() == (_userManager.GetUserId(User) ?? "").ToLower())
    //         .Select(o => o.CalendarId)
    //         .ToListAsync());
    //     ViewBag.OwnerTypes = new SelectList(new List<bool> { false, true });
    //     return View();
    // }
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> SendCalendar([Bind("UserId,CalendarId,IsOwner")] Ownership @ownership) {
    //     
    //     Console.WriteLine("-----------------");
    //     Console.WriteLine(@ownership.UserId);
    //     Console.WriteLine(@ownership.CalendarId);
    //     Console.WriteLine(@ownership.IsOwner);
    //     Console.WriteLine("-----------------");
    //
    //     var list = await _context.Ownership
    //         .Where(o => o.UserId.ToString().ToLower() == ownership.UserId.ToString().ToLower())
    //         .Where(x => x.CalendarId == @ownership.CalendarId)
    //         .ToListAsync();
    //     if(!list.IsNullOrEmpty()) {
    //         Console.WriteLine("----------------------Already have this calendar.");
    //         return RedirectToAction(nameof(ShowUsers));
    //     }
    //     if (!ModelState.IsValid) {
    //         foreach (var state in ModelState)
    //         {
    //             if (state.Value.Errors.Any())
    //             {
    //                 Console.WriteLine($"Key: {state.Key}, Errors: {string.Join(",", state.Value.Errors.Select(e => e.ErrorMessage))}");
    //             }
    //         }
    //         Console.WriteLine("-----------------------Third if");
    //         return RedirectToAction(nameof(ShowUsers));
    //     }
    //     
    //     _context.Ownership.Add(@ownership);
    //     await _context.SaveChangesAsync();
    //     return RedirectToAction(nameof(ShowUsers));
    //     
    // }
}