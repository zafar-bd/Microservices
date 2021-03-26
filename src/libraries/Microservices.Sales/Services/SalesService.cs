using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Common.Messages;
using Microservices.Sales.Data.Context;
using Microservices.Sales.Data.Domains;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Sales.Services
{
    public class SalesService : ISalesService
    {
        private readonly SalesDbContext _dbContext;
        private readonly IStockService _stockService;

        public SalesService(SalesDbContext dbContext, IStockService stockService)
        {
            _dbContext = dbContext;
            this._stockService = stockService;
        }
        public async Task<Data.Domains.Sales> SaveSalesAsync(SalesCommandReceived dto)
        {
            var products = await _stockService.GetAvailableStockProductAsync(dto.SoldItems);

            decimal totalAmountToPay = 0;
            Data.Domains.Sales salesToSave = new();
            List<SalesDetails> salesItemsToSave = new();
            if (!await _dbContext.Customers.AnyAsync(c => c.Id == dto.CustomerId))
            {
                salesToSave.Customer = new Customer
                {
                    Id = dto.CustomerId,
                    Email = dto.Email,
                    Mobile = dto.Mobile,
                    Name = dto.CustomerName,
                };
                await _dbContext.Customers.AddAsync(salesToSave.Customer);
            }

            dto.SoldItems.ForEach(c =>
            {
                var productToUpdate = products.FirstOrDefault(p => p.Id == c.ProductId);
                var price = productToUpdate.Price * c.Qty;
                productToUpdate.StockQty -= c.Qty;
                totalAmountToPay += price;
                salesItemsToSave.Add(new SalesDetails
                {
                    Price = price,
                    ProductId = c.ProductId,
                    Qty = c.Qty
                });
                c.ProductName = productToUpdate.Name;
                _dbContext.Products.Update(productToUpdate);
            });

            salesToSave.CustomerId = dto.CustomerId;
            salesToSave.TotalPrice = totalAmountToPay;
            salesToSave.CustomerId = dto.CustomerId;
            salesToSave.SoldAt = DateTimeOffset.UtcNow;
            salesToSave.Reference = dto.Reference;
            salesToSave.SalesDetails = salesItemsToSave;
            
            await _dbContext.Sales.AddAsync(salesToSave);
            await _dbContext.SaveChangesAsync();

            return salesToSave;
        }
    }
}
