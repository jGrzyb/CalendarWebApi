﻿namespace projekt.Models;

public class Ownership {
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int CalendarId { get; set; }
    public bool IsOwner { get; set; }
}