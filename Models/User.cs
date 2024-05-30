using System.ComponentModel.DataAnnotations;

namespace projekt.Models;

public class User {
    [Key]
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public ICollection<Event> Events { get; set; }
}