using MassTransit;
using MediatR;
using Microservices.Common.Exceptions;
using Microservices.Common.Messages;
using Microservices.Order.Data.Context;
using Microservices.Order.Data.Domains;
using Microservices.Order.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Order.Cqrs.Commands
{
    public class OrderCommandHandlers :
        INotificationHandler<ShoppingCartDto>
    {
        private readonly OrderDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderCommandHandlers(OrderDbContext dbContext,
            IPublishEndpoint publishEndpoint)
        {
            this._dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Handle(ShoppingCartDto dto, CancellationToken cancellationToken)
        {
            var products = await _dbContext
                .Products
                .Where(p => dto.CartItemDtos
                               .Select(c => c.ProductId)
                               .Any(c => c == p.Id))
                .ToListAsync();

            if (!products.All(p => dto.CartItemDtos
                                      .Select(c => c.ProductId)
                                      .Any(c => c == p.Id)
                                && dto.CartItemDtos
                                      .Select(c => c.Qty)
                                      .Any(q => (p.StockQty - p.HoldQty) >= q)))
            {
                throw new BadRequestException("Out of Stock");
            }

            decimal totalAmountToPay = 0;
            Data.Domains.Order orderToSave = new();
            List<OrderItem> orderItemsToSave = new();
            List<StockUpdated> stockUpdatedEventMessage = new();
            OrderCreated orderCreatedEventMessage = new();
            bool isNewCustomer = false;
            if (await _dbContext.Customers.AnyAsync(c => c.Id == dto.CustomerId))
            {
                isNewCustomer = true;
                orderToSave.Customer = new Customer
                {
                    Id = dto.CustomerId,
                    Email = dto.Email,
                    Mobile = dto.Mobile,
                    Name = dto.CustomerName,
                };
            }

            dto.CartItemDtos.ForEach(c =>
            {
                var productToUpdate = products.Find(p => p.Id == c.ProductId);
                var price = productToUpdate.Price * c.Qty;
                productToUpdate.StockQty -= c.Qty;
                productToUpdate.HoldQty += c.Qty;
                totalAmountToPay += price;
                orderItemsToSave.Add(new OrderItem
                {
                    Price = price,
                    ProductId = c.ProductId,
                    Qty = c.Qty
                });
                stockUpdatedEventMessage.Add(new StockUpdated
                {
                    StockQty = c.Qty,
                    HoldQty = c.Qty,
                    ProductId = c.ProductId,
                });

                orderCreatedEventMessage.OrderItemsCreated.Add(new OrderItemsCreated
                {
                    ProductId = c.ProductId,
                    Qty = c.Qty,
                    Price = price,
                    ProductName = productToUpdate.Name
                });


            });

            // product updated
            orderToSave.CustomerId = dto.CustomerId;
            orderToSave.AmountToPay = totalAmountToPay;
            orderToSave.CustomerId = dto.CustomerId;
            orderToSave.OrderdAt = DateTimeOffset.UtcNow;
            orderToSave.ShipmentAddress = dto.ShippingAddress;
            orderToSave.OrderItems = orderItemsToSave;

            orderCreatedEventMessage.CustomerId = dto.CustomerId;
            orderCreatedEventMessage.AmountToPay = totalAmountToPay;
            orderCreatedEventMessage.CustomerName = dto.CustomerName;
            orderCreatedEventMessage.Email = dto.Email;
            orderCreatedEventMessage.Mobile = dto.Email;
            orderCreatedEventMessage.ShippingAddress = dto.ShippingAddress;

            await _dbContext.Orders.AddAsync(orderToSave);
            _dbContext.Products.UpdateRange(products);
            await _dbContext.SaveChangesAsync();

            if (isNewCustomer)
            {
                await _publishEndpoint.Publish(new CustomerCreated
                {
                    Id = dto.CustomerId,
                    Address = dto.ShippingAddress,
                    Email = dto.Email,
                    Mobile = dto.Mobile,
                    Name = dto.CustomerName,
                });
            }

            await _publishEndpoint.Publish(stockUpdatedEventMessage);
            await _publishEndpoint.Publish(orderCreatedEventMessage);

        }
    }
}
