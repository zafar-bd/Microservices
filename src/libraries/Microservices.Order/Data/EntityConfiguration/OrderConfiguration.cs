using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Order.Data.EntityConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<ViewModels.OrderViewModel>
    {
        public void Configure(EntityTypeBuilder<ViewModels.OrderViewModel> builder)
        {
            builder.Property(o => o.ShipmentAddress).HasMaxLength(500);
            builder.Property(o => o.DeliveredBy).HasMaxLength(100);
            builder.Property(o => o.ReceivedBy).HasMaxLength(100);
        }
    }
}
