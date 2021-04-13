using MassTransit;
using Microservices.Common.Messages;
using Microservices.Order.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Processor.Worker
{
    public class OrderConsumer : IConsumer<OrderReceived>
    {
        ILogger<OrderConsumer> _logger;
        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderConsumer(
            ILogger<OrderConsumer> logger,
            IOrderService orderService,
            IPublishEndpoint publishEndpoint)
        {
            this._logger = logger;
            this._orderService = orderService;
            this._publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderReceived> context)
        {
            try
            {
                OrderReceived dto = context.Message;
                var savedOrder = await _orderService.SaveOrderAsync(dto);
                await this.ProcessOrderAsync(savedOrder, dto);
                _logger.LogInformation("Order Processed");
            }
            catch (Exception ex)
            {
                await _publishEndpoint.Publish(new GlobalExceptionMessage
                {
                    ApplicationName = "Order Processor",
                    OccurredAt = DateTimeOffset.UtcNow,
                    UserName = context.Message.CustomerId.ToString(),
                    ExceptionMessage = ex.Message,
                    FunctionName = "Order Save",
                    InnerExceptionMessage = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace
                });

                await _publishEndpoint.Publish(new Notification
                {
                    UserId = context.Message.CustomerId,
                    Message = "Order Request Failed",
                    MessageFrom = "Order Processor",
                    MessageSent = DateTimeOffset.UtcNow,
                    IsError = true
                });

                throw;
            }
        }

        private async Task ProcessOrderAsync(Microservices.Order.Data.Domains.Order savedOrder, OrderReceived dto)
        {
            ProductUpdated productUpdatedEventMessage = new();
            OrderCreated orderCreatedEventMessage = new();
            CustomerCreated customerCreatedEventMessage = new();
            Notification notificationEventMessage = new();

            if (savedOrder.Customer is not null)
            {
                customerCreatedEventMessage.Id = savedOrder.CustomerId;
                customerCreatedEventMessage.Address = dto.ShippingAddress;
                customerCreatedEventMessage.Email = dto.Email;
                customerCreatedEventMessage.Mobile = dto.Mobile;
                customerCreatedEventMessage.Name = dto.CustomerName;
            }

            decimal totalAmountToPay = 0;

            savedOrder.OrderItems.ToList().ForEach(c =>
            {
                var product = dto.OrderReceivedItems.Find(p => p.ProductId == c.ProductId);
                var price = c.Price * c.Qty;
                totalAmountToPay += price;
                productUpdatedEventMessage.UpdatedItems.Add(new UpdatedItem
                {
                    HoldQty = c.Qty,
                    ProductId = c.ProductId
                });

                orderCreatedEventMessage.OrderItemsCreated.Add(new OrderItemsCreated
                {
                    ProductId = c.ProductId,
                    Qty = c.Qty,
                    Price = price,
                    ProductName = product.ProductName
                });
            });

            orderCreatedEventMessage.Id = savedOrder.Id;
            orderCreatedEventMessage.CustomerId = dto.CustomerId;
            orderCreatedEventMessage.AmountToPay = totalAmountToPay;
            orderCreatedEventMessage.CustomerName = dto.CustomerName;
            orderCreatedEventMessage.Email = dto.Email;
            orderCreatedEventMessage.Mobile = dto.Email;
            orderCreatedEventMessage.ShippingAddress = dto.ShippingAddress;

            notificationEventMessage.UserId = dto.CustomerId;
            notificationEventMessage.Message = "Order Processed";
            notificationEventMessage.MessageFor = dto.CustomerId.ToString();
            notificationEventMessage.MessageSent = DateTimeOffset.UtcNow;
            notificationEventMessage.MessageFrom = "Order Processor";

            await _publishEndpoint.Publish(notificationEventMessage);
            await _publishEndpoint.Publish(orderCreatedEventMessage);
            productUpdatedEventMessage.ProductUpdatedFrom = ProductUpdatedFrom.OrderService;
            await _publishEndpoint.Publish(productUpdatedEventMessage);

            if (savedOrder.Customer is not null)
                await _publishEndpoint.Publish(customerCreatedEventMessage);
        }
    }
}
