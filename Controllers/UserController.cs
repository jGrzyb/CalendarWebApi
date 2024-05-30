using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly DataBaseContext _context;

    public UserController(DataBaseContext context)
    {
        _context = context;
    }

    [HttpGet, ActionName("Index")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Users.ToListAsync());
    }

    [HttpGet, ActionName("Details")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpGet, ActionName("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost, ActionName("Create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Email,Password")] User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }
        
        _context.Add(user);
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

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password")] User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(user);
        }
        
        try
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(user.Id))
            {
                return NotFound();
            }

            throw;
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

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}