using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;

namespace projekt.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class TaskEventController : ControllerBase {
    private readonly DataBaseContext _context;

    public TaskEventController(DataBaseContext context) {
        _context = context;
    }

    // GET: api/TaskEvent
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskEvent>>> GetTaskEvent() {
        return await _context.TaskEvent.ToListAsync();
    }

    // GET: api/TaskEvent/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskEvent>> GetTaskEvent(int id) {
        var taskEvent = await _context.TaskEvent.FindAsync(id);

        if (taskEvent == null) {
            return NotFound();
        }

        return taskEvent;
    }

    // PUT: api/TaskEvent/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTaskEvent(int id, TaskEvent taskEvent) {
        if (id != taskEvent.Id) {
            return BadRequest();
        }

        _context.Entry(taskEvent).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!TaskEventExists(id)) {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/TaskEvent
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TaskEvent>> PostTaskEvent(TaskEvent taskEvent) {
        _context.TaskEvent.Add(taskEvent);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTaskEvent", new { id = taskEvent.Id }, taskEvent);
    }

    // DELETE: api/TaskEvent/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskEvent(int id) {
        var taskEvent = await _context.TaskEvent.FindAsync(id);
        if (taskEvent == null) {
            return NotFound();
        }

        _context.TaskEvent.Remove(taskEvent);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TaskEventExists(int id) {
        return _context.TaskEvent.Any(e => e.Id == id);
    }
}