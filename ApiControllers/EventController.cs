using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase {
    private readonly DataBaseContext _context;

    public EventController(DataBaseContext context) {
        _context = context;
    }

    // GET: api/Event
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvent() {
        return await _context.Event.ToListAsync();
    }

    // GET: api/Event/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(int id) {
        var @event = await _context.Event.FindAsync(id);

        if (@event == null) return NotFound();

        return @event;
    }

    // PUT: api/Event/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvent(int id, Event @event) {
        if (id != @event.Id) return BadRequest();

        _context.Entry(@event).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!EventExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Event
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Event>> PostEvent(Event @event) {
        _context.Event.Add(@event);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
    }

    // DELETE: api/Event/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id) {
        var @event = await _context.Event.FindAsync(id);
        if (@event == null) return NotFound();

        _context.Event.Remove(@event);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EventExists(int id) {
        return _context.Event.Any(e => e.Id == id);
    }
}