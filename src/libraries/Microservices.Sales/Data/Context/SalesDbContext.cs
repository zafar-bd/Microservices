using System.Reflection;
using Microservices.Common.Helpers;
using Microservices.Sales.Data.Domains;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Sales.Data.Context
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options)
              : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<Domains.Sales> Sales { get; set; }
        public DbSet<SalesDetails> SalesDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ManageNonEntities();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var entities = modelBuilder.Model.GetEntityTypes();
            entities.ManageDeletingBehaviorRestrictions();
            entities.ManageDecimalPrecision();
        }
    }
}
