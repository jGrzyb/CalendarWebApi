namespace projekt.Models;

public class EventShow {
    public int Id { get; set; }
    public int CalendarId { get; set; }
    public string CalendarName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime Date { get; set; }
    public DateTime EndDate { get; set; }
}