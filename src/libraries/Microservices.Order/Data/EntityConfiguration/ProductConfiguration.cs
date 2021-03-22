using Microservices.Order.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Order.Data.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductViewModel>
    {
        public void Configure(EntityTypeBuilder<ProductViewModel> builder)
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
