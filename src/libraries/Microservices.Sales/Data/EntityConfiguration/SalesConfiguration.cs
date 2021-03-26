using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Sales.Data.EntityConfiguration
{
    public class SalesConfiguration : IEntityTypeConfiguration<Domains.Sales>
    {
        public void Configure(EntityTypeBuilder<Domains.Sales> builder)
        {
            builder.Property(o => o.OperatedBy).HasMaxLength(100);
            builder.Property(o => o.Reference).HasMaxLength(100);
        }
    }
}
