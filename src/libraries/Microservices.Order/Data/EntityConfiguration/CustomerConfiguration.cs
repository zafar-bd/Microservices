using Microservices.Order.Data.Domains;
using Microservices.Order.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Order.Data.EntityConfiguration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.Email).HasMaxLength(100);
            builder.Property(p => p.Mobile).HasMaxLength(11);
            builder.Property(p => p.Address).HasMaxLength(500);
            builder.HasAlternateKey(l => l.Mobile);
            builder.HasAlternateKey(l => l.Email);

        }
    }
}
