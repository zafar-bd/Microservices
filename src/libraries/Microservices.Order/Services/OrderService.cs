using Microservices.Common.Exceptions;
using Microservices.Common.Messages;
using Microservices.Order.Data.Context;
using Microservices.Order.Data.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Order.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _dbContext;
        public OrderService(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Data.Domains.Order> SaveOrderAsync(OrderReceived dto)
        {
            var products = await _dbContext
           .Products
           .Where(p => dto.OrderReceivedItems.Select(o => o.ProductId).Contains(p.Id))
           .ToListAsync();

            foreach (var product in products)
            {
                foreach (var item in dto.OrderReceivedItems)
                {
                    if (product.Id == item.ProductId && (product.StockQty - product.HoldQty) < item.Qty)
                    {
                        throw new BadRequestException("Out of Stock");
                    }
                }
            }

            decimal totalAmountToPay = 0;
            Data.Domains.Order orderToSave = new();
            List<OrderItem> orderItemsToSave = new();
            if (!await _dbContext.Customers.AnyAsync(c => c.Id == dto.CustomerId))
            {
                orderToSave.Customer = new Customer
                {
                    Id = dto.CustomerId,
                    Email = dto.Email,
                    Mobile = dto.Mobile,
                    Name = dto.CustomerName,
                };
            }

            dto.OrderReceivedItems.ForEach(c =>
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
                c.ProductName = productToUpdate.Name;
                _dbContext.Products.Update(productToUpdate);
            });

            orderToSave.CustomerId = dto.CustomerId;
            orderToSave.AmountToPay = totalAmountToPay;
            orderToSave.CustomerId = dto.CustomerId;
            orderToSave.OrderdAt = DateTimeOffset.UtcNow;
            orderToSave.ShipmentAddress = dto.ShippingAddress;
            orderToSave.OrderItems = orderItemsToSave;
           
            await _dbContext.Orders.AddAsync(orderToSave);
            await _dbContext.SaveChangesAsync();
            
            return orderToSave;
        }
    }
}
