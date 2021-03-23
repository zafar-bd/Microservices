using MassTransit;
using Microservices.Common.EventChannel;
using Microservices.Common.Messages;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Common.BackgroundServices
{
    public class GlobalExceptionBackgroundService : BackgroundService
    {
        private readonly IBus _publishEndpoint;

        public GlobalExceptionBackgroundService(IBus publishEndpoint)
        {
            this._publishEndpoint = publishEndpoint;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var exception = await EventDispatchChannel<GlobalExceptionMessage>.PullAsync();
                await _publishEndpoint.Publish(exception);
            }
        }
    }
}
