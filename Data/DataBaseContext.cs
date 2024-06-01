using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using projekt.Models;

namespace projekt.Data
{
    public partial class DataBaseContext : DbContext
    {
        public DbSet<projekt.Models.Calendar> Calendar { get; set; } = default!;
        public DbSet<projekt.Models.Event> Event { get; set; } = default!;
        public DbSet<projekt.Models.Ownership> Ownership { get; set; } = default!;
        public DbSet<projekt.Models.TaskEvent> TaskEvent { get; set; } = default!;

        public DataBaseContext()
        {
        }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlite("Data Source=Data.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}





// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using projekt.Models;
//
// namespace projekt.Data
// {
//     public class DataBaseContext : DbContext
//     {
//         public DataBaseContext (DbContextOptions<DataBaseContext> options)
//             : base(options)
//         {
//         }
//
//         
//     }
// }
