using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt.Models;

public class Event {
    [Key] public int Id { get; set; }
    [ForeignKey("Calendar")] public int CalendarId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public DateTime EndDate { get; set; }
}