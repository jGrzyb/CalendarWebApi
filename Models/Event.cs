using System.ComponentModel.DataAnnotations.Schema;

public class Event
{
    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}