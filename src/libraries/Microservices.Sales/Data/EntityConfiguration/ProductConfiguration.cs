using Microservices.Sales.Data.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Sales.Data.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(250);
            builder
                .HasIndex(l => new
                {
                    l.Name,
                    l.ProductCategoryId
                })
                .IsUnique();

        }
    }
}
