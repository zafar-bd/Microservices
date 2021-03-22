using Microservices.Order.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Order.Data.EntityConfiguration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemViewModel>
    {
        public void Configure(EntityTypeBuilder<OrderItemViewModel> builder)
        {
            builder
                .HasIndex(l => new
                {
                    l.ProductId,
                    l.OrderId
                })
                .IsUnique();

        }
    }
}
