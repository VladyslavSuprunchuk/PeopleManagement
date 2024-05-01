using Microsoft.EntityFrameworkCore;
using PeopleManagement.DatabaseProvider.Models;

namespace PeopleManagement.DatabaseProvider
{
    public class DataContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DataContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Database=CatalogDb;Port=5432;User Id=postgres;Password=admin2341");
        }
    }
}
