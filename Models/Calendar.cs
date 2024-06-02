using System.ComponentModel.DataAnnotations;

namespace projekt.Models;

public class Calendar {
    [Key] public int Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
}