using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.Controllers
{
    public class OwnershipController : Controller
    {
        private readonly DataBaseContext _context;

        public OwnershipController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Ownership
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ownership.ToListAsync());
        }

        // GET: Ownership/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownership = await _context.Ownership
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ownership == null)
            {
                return NotFound();
            }

            return View(ownership);
        }

        // GET: Ownership/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ownership/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CalendarId")] Ownership ownership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ownership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ownership);
        }

        // GET: Ownership/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownership = await _context.Ownership.FindAsync(id);
            if (ownership == null)
            {
                return NotFound();
            }
            return View(ownership);
        }

        // POST: Ownership/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CalendarId")] Ownership ownership)
        {
            if (id != ownership.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ownership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnershipExists(ownership.Id))
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
            return View(ownership);
        }

        // GET: Ownership/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownership = await _context.Ownership
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ownership == null)
            {
                return NotFound();
            }

            return View(ownership);
        }

        // POST: Ownership/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ownership = await _context.Ownership.FindAsync(id);
            if (ownership != null)
            {
                _context.Ownership.Remove(ownership);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnershipExists(int id)
        {
            return _context.Ownership.Any(e => e.Id == id);
        }
    }
}
