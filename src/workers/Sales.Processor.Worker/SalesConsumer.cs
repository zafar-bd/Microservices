using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microservices.Common.Messages;
using Microservices.Sales.Services;
using Microsoft.Extensions.Logging;

namespace Sales.Processor.Worker
{
    public class SalesConsumer : IConsumer<SalesCommandReceived>
    {
        ILogger<SalesConsumer> _logger;
        private readonly ISalesService _salesService;
        private readonly IPublishEndpoint _publishEndpoint;

        public SalesConsumer(
            ILogger<SalesConsumer> logger,
            ISalesService salesService,
            IPublishEndpoint publishEndpoint)
        {
            this._logger = logger;
            this._salesService = salesService;
            this._publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<SalesCommandReceived> context)
        {
            try
            {
                SalesCommandReceived dto = context.Message;
                var savedSales = await _salesService.SaveSalesAsync(dto);
                await this.ProcessSalesAsync(savedSales, dto);
                _logger.LogInformation("Sales Processed");
            }
            catch (Exception ex)
            {
                await _publishEndpoint.Publish(new GlobalExceptionMessage
                {
                    ApplicationName = "Sales Processor",
                    OccurredAt = DateTimeOffset.UtcNow,
                    UserName = context.Message.CustomerId.ToString(),
                    ExceptionMessage = ex.Message,
                    FunctionName = "Sales Save",
                    InnerExceptionMessage = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace
                });

                await _publishEndpoint.Publish(new Notification
                {
                    UserId = context.Message.CustomerId,
                    Message = "Sales Request Failed",
                    MessageFrom = "Sales Processor",
                    MessageSent = DateTimeOffset.UtcNow,
                    IsError = true
                });

                throw;
            }
        }

        private async Task ProcessSalesAsync(Microservices.Sales.Data.Domains.Sales savedSales, SalesCommandReceived dto)
        {
            ProductUpdated productUpdatedEventMessage = new();
            SalesCreated salesCreatedEventMessage = new();
            CustomerCreated customerCreatedEventMessage = new();
            Notification notificationEventMessage = new();
            OrderStatusUpdated orderStatusUpdatedEventMessage = new();

            if (savedSales.Customer is not null)
            {
                customerCreatedEventMessage.Id = savedSales.CustomerId;
                customerCreatedEventMessage.Email = dto.Email;
                customerCreatedEventMessage.Mobile = dto.Mobile;
                customerCreatedEventMessage.Name = dto.CustomerName;
            }

            decimal totalAmountToPay = 0;
            savedSales.SalesDetails.ToList().ForEach(c =>
            {
                var product = dto.SoldItems.Find(p => p.ProductId == c.ProductId);
                var price = c.Price * c.Qty;
                totalAmountToPay += price;
                productUpdatedEventMessage.UpdatedItems.Add(new UpdatedItem
                {
                    StockQty = -c.Qty,
                    HoldQty = -c.Qty,
                    ProductId = c.ProductId
                });

                salesCreatedEventMessage.SalesItemsCreated.Add(new SalesItemsCreated
                {
                    ProductId = c.ProductId,
                    Qty = c.Qty,
                    Price = price,
                    ProductName = product.ProductName
                });
            });

            salesCreatedEventMessage.Id = savedSales.Id;
            salesCreatedEventMessage.CustomerId = dto.CustomerId;
            salesCreatedEventMessage.AmountToPay = totalAmountToPay;
            salesCreatedEventMessage.CustomerName = dto.CustomerName;
            salesCreatedEventMessage.Email = dto.Email;
            salesCreatedEventMessage.Mobile = dto.Email;

            notificationEventMessage.UserId = dto.CustomerId;
            notificationEventMessage.Message = "Sales Processed";
            notificationEventMessage.MessageFor = dto.CustomerId.ToString();
            notificationEventMessage.MessageSent = DateTimeOffset.UtcNow;
            notificationEventMessage.MessageFrom = "Sales Processor";

            await _publishEndpoint.Publish(notificationEventMessage);
            await _publishEndpoint.Publish(salesCreatedEventMessage);
            productUpdatedEventMessage.ProductUpdatedFrom = ProductUpdatedFrom.SalesService;
            await _publishEndpoint.Publish(productUpdatedEventMessage);

            if (savedSales.Reference is not null)
            {
                orderStatusUpdatedEventMessage.OrderId = Guid.Parse(savedSales.Reference);
                orderStatusUpdatedEventMessage.IsDelivered = true;
                orderStatusUpdatedEventMessage.SalesId = savedSales.Id;
                await _publishEndpoint.Publish(orderStatusUpdatedEventMessage);
            }

            if (savedSales.Customer is not null)
                await _publishEndpoint.Publish(customerCreatedEventMessage);
        }
    }
}
