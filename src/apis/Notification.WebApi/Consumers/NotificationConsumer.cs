using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Notification.WebApi.Hubs;
using System.Threading.Tasks;

namespace Notification.WebApi.Consumers
{
    public class NotificationConsumer : IConsumer<Microservices.Common.Messages.Notification>
    {
        ILogger<NotificationConsumer> _logger;
        private readonly IHubContext<NotificationHub> _hub;
        public NotificationConsumer(
            ILogger<NotificationConsumer> logger,
            IHubContext<NotificationHub> hub)
        {
            _logger = logger;
            _hub = hub;
        }

        public async Task Consume(ConsumeContext<Microservices.Common.Messages.Notification> context)
        {
            await _hub.Clients.All.SendAsync(context.Message.UserId.ToString(), context.Message);
        }
    }
}
