using Microsoft.EntityFrameworkCore;
using UnitOfWork.Test.Entities;

namespace UnitOfWork.Test
{
    public class InMemoryContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("test");
        }
    }
}
