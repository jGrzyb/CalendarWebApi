using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class CalendarController : ControllerBase {
    private readonly DataBaseContext _context;

    public CalendarController(DataBaseContext context) {
        _context = context;
    }

    // GET: api/Calendar
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Calendar>>> GetCalendar() {
        return await _context.Calendar.ToListAsync();
    }

    // GET: api/Calendar/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Calendar>> GetCalendar(int id) {
        var calendar = await _context.Calendar.FindAsync(id);

        if (calendar == null) return NotFound();

        return calendar;
    }

    // PUT: api/Calendar/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCalendar(int id, Calendar calendar) {
        if (id != calendar.Id) return BadRequest();

        _context.Entry(calendar).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!CalendarExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Calendar
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Calendar>> PostCalendar(Calendar calendar) {
        _context.Calendar.Add(calendar);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCalendar", new { id = calendar.Id }, calendar);
    }

    // DELETE: api/Calendar/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCalendar(int id) {
        var calendar = await _context.Calendar.FindAsync(id);
        if (calendar == null) return NotFound();

        _context.Calendar.Remove(calendar);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CalendarExists(int id) {
        return _context.Calendar.Any(e => e.Id == id);
    }
}