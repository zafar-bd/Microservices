using MassTransit;
using Microservices.Exceptions.Data.Context;
using Microservices.Exceptions.Data.Domains;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Application.Exception.Worker
{
    public class ExceptionConsumer : IConsumer<Microservices.Common.Messages.GlobalExceptionMessage>
    {
        ILogger<ExceptionConsumer> _logger;
        private readonly ExceptionDbContext _dbContext;

        public ExceptionConsumer(ILogger<ExceptionConsumer> logger, ExceptionDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<Microservices.Common.Messages.GlobalExceptionMessage> context)
        {
            await _dbContext.GlobalExceptionMessages.AddAsync(new GlobalExceptionMessage
            {
                ApplicationName = context.Message.ApplicationName,
                ExceptionMessage = context.Message.ExceptionMessage,
                FunctionName = context.Message.FunctionName,
                InnerExceptionMessage = context.Message.InnerExceptionMessage,
                OccurredAt = context.Message.OccurredAt,
                StackTrace = context.Message.StackTrace,
                UserName = context.Message.UserName
            });
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation(context.Message.ExceptionMessage);
        }
    }
}
