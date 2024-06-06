using System.ComponentModel.DataAnnotations.Schema;

namespace projekt.Models;

public class TaskEvent {
    public int Id { get; set; }

    [ForeignKey("Calendar")] public int CalendarId { get; set; }

    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}