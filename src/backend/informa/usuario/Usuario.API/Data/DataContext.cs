using Microsoft.EntityFrameworkCore;
using Usuario.API.Models;

namespace Usuario.API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Usuarios { get; set; }
        public DataContext(){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
