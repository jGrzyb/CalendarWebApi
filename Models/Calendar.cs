﻿namespace projekt.Models;

public class Calendar {
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsPublic { get; set; }
}