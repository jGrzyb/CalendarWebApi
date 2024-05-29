using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext (DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;

        public DbSet<Event> Event { get; set; } = default!;
    }
}
