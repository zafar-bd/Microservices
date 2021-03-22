using Microservices.Product.Data.Domains;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Order.Data.Context
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
              : base(options)
        {

        }

        public DbSet<Product.Data.Domains.Product> Products { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
    }
}
