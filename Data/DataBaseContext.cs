using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projekt.Models;

namespace projekt.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext (DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<projekt.Models.Calendar> Calendar { get; set; } = default!;
        public DbSet<projekt.Models.Event> Event { get; set; } = default!;
        public DbSet<projekt.Models.Ownership> Ownership { get; set; } = default!;
        public DbSet<projekt.Models.TaskEvent> TaskEvent { get; set; } = default!;
    }
}
