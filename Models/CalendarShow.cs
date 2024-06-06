namespace projekt.Models;

public class CalendarShow {
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsPublic { get; set; }
    public bool IsOwner { get; set; }
}