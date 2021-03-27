using System.Threading.Tasks;
using MassTransit;
using Microservices.Common.Cache;
using Microservices.Order.Data.Context;
using Microsoft.Extensions.Logging;

namespace Order.WebApi.Consumers
{
    public class OrderStatusConsumer : IConsumer<Microservices.Common.Messages.OrderStatusUpdated>
    {
        ILogger<OrderStatusConsumer> _logger;
        private readonly OrderDbContext _dbContext;
        private readonly ICacheHelper _redisCacheClient;
        public OrderStatusConsumer(
            ILogger<OrderStatusConsumer> logger,
            OrderDbContext dbContext, ICacheHelper redisCacheClient)
        {
            _logger = logger;
            _dbContext = dbContext;
            _redisCacheClient = redisCacheClient;
        }

        public async Task Consume(ConsumeContext<Microservices.Common.Messages.OrderStatusUpdated> context)
        {
            var order = await _dbContext.Orders.FindAsync(context.Message.OrderId);
            if (order is not null)
            {
                order.IsDelivered = context.Message.IsDelivered;
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
                await _redisCacheClient.RemoveAsync($"order-{order.Id}");
            }
        }
    }
}
