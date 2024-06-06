using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.Controllers;

public class TaskEventController : Controller {
    private readonly DataBaseContext _context;

    public TaskEventController(DataBaseContext context) {
        _context = context;
    }

    // GET: TaskEvent
    public async Task<IActionResult> Index() {
        return View(await _context.TaskEvent.ToListAsync());
    }

    // GET: TaskEvent/Details/5
    public async Task<IActionResult> Details(int? id) {
        if (id == null) {
            return NotFound();
        }

        var taskEvent = await _context.TaskEvent
            .FirstOrDefaultAsync(m => m.Id == id);
        if (taskEvent == null) {
            return NotFound();
        }

        return View(taskEvent);
    }

    // GET: TaskEvent/Create
    public IActionResult Create() {
        return View();
    }

    // POST: TaskEvent/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CalendarId,Date,Name,Description")] TaskEvent taskEvent) {
        if (ModelState.IsValid) {
            _context.Add(taskEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(taskEvent);
    }

    // GET: TaskEvent/Edit/5
    public async Task<IActionResult> Edit(int? id) {
        if (id == null) {
            return NotFound();
        }

        var taskEvent = await _context.TaskEvent.FindAsync(id);
        if (taskEvent == null) {
            return NotFound();
        }

        return View(taskEvent);
    }

    // POST: TaskEvent/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CalendarId,Date,Name,Description")] TaskEvent taskEvent) {
        if (id != taskEvent.Id) {
            return NotFound();
        }

        if (ModelState.IsValid) {
            try {
                _context.Update(taskEvent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!TaskEventExists(taskEvent.Id)) {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(taskEvent);
    }

    // GET: TaskEvent/Delete/5
    public async Task<IActionResult> Delete(int? id) {
        if (id == null) {
            return NotFound();
        }

        var taskEvent = await _context.TaskEvent
            .FirstOrDefaultAsync(m => m.Id == id);
        if (taskEvent == null) {
            return NotFound();
        }

        return View(taskEvent);
    }

    // POST: TaskEvent/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        var taskEvent = await _context.TaskEvent.FindAsync(id);
        if (taskEvent != null) {
            _context.TaskEvent.Remove(taskEvent);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TaskEventExists(int id) {
        return _context.TaskEvent.Any(e => e.Id == id);
    }
}