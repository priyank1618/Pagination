using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace MvcCoreTutorial.Models.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> opts) : base(opts)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Pagination;Username=postgres;Password=3117");


        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Employee> Employee { get; set; }
    }
}
