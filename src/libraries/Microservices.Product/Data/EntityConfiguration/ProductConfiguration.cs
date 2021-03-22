using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Product.Data.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Domains.Product>
    {
        public void Configure(EntityTypeBuilder<Domains.Product> builder)
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
