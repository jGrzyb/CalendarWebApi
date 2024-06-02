using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class OwnershipController : ControllerBase {
    private readonly DataBaseContext _context;

    public OwnershipController(DataBaseContext context) {
        _context = context;
    }

    // GET: api/Ownership
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ownership>>> GetOwnership() {
        return await _context.Ownership.ToListAsync();
    }

    // GET: api/Ownership/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Ownership>> GetOwnership(int id) {
        var ownership = await _context.Ownership.FindAsync(id);

        if (ownership == null) return NotFound();

        return ownership;
    }

    // PUT: api/Ownership/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOwnership(int id, Ownership ownership) {
        if (id != ownership.Id) return BadRequest();

        _context.Entry(ownership).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!OwnershipExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Ownership
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Ownership>> PostOwnership(Ownership ownership) {
        _context.Ownership.Add(ownership);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetOwnership", new { id = ownership.Id }, ownership);
    }

    // DELETE: api/Ownership/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOwnership(int id) {
        var ownership = await _context.Ownership.FindAsync(id);
        if (ownership == null) return NotFound();

        _context.Ownership.Remove(ownership);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OwnershipExists(int id) {
        return _context.Ownership.Any(e => e.Id == id);
    }
}