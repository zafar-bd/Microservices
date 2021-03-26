using Microservices.Sales.Data.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Sales.Data.EntityConfiguration
{
    public class SoldItemConfiguration : IEntityTypeConfiguration<SalesDetails>
    {
        public void Configure(EntityTypeBuilder<SalesDetails> builder)
        {
            builder
                .HasIndex(l => new
                {
                    l.ProductId,
                    OrderId = l.SalesId
                })
                .IsUnique();

        }
    }
}
