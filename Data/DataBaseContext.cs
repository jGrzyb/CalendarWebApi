using Microsoft.EntityFrameworkCore;
using projekt.Models;

namespace projekt.Data;

public class DataBaseContext : DbContext
{
    public DataBaseContext (DbContextOptions<DataBaseContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = default!;

    public DbSet<Event> Events { get; set; } = default!;
}