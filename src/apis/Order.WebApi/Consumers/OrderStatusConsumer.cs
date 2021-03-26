using System.Threading.Tasks;
using MassTransit;
using Microservices.Order.Data.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Order.WebApi.Consumers
{
    public class OrderStatusConsumer : IConsumer<Microservices.Common.Messages.OrderStatusUpdated>
    {
        ILogger<OrderStatusConsumer> _logger;
        private readonly OrderDbContext _dbContext;

        public OrderStatusConsumer(
            ILogger<OrderStatusConsumer> logger,
            OrderDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<Microservices.Common.Messages.OrderStatusUpdated> context)
        {
            var order = await _dbContext.Orders.FindAsync(context.Message.OrderId);
            if (order is not null)
            {
                order.IsDelivered = context.Message.IsDelivered;
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
