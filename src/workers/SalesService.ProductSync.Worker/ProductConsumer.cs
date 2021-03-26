﻿using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microservices.Common.Messages;
using Microservices.Sales.Data.Context;
using Microservices.Sales.Data.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SalesService.ProductSync.Worker
{
    public class ProductConsumer : IConsumer<ProductUpdated>
    {
        ILogger<ProductConsumer> _logger;
        private readonly SalesDbContext _dbContext;

        public ProductConsumer(ILogger<ProductConsumer> logger, SalesDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ProductUpdated> context)
        {
            await this.HandleProductsAsync(context.Message);
            _logger.LogInformation("Products for Sales Service has been consumed");
        }

        private async Task HandleProductsAsync(ProductUpdated message)
        {
            var productsFromDb = await _dbContext.Products
                .Where(p => message.UpdatedItems.Select(i => i.ProductId).Contains(p.Id))
                .ToListAsync();

            foreach (var product in productsFromDb.OrderBy(p => p.Id))
            {
                foreach (var dto in message.UpdatedItems.OrderBy(p => p.ProductId))
                {
                    if (product.Id != dto.ProductId)
                        await this.CreateProductAsync(dto);
                    else
                        this.UpdateProduct(dto, product);
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        private void UpdateProduct(UpdatedItem dto, Product product)
        {
            if (!string.IsNullOrEmpty(dto.ProductName))
                product.Name = dto.ProductName;
            if (dto.StockQty is not null)
                product.StockQty += dto.StockQty ?? 0;
            if (dto.Price is not null)
                product.Price = (decimal)dto.Price;
            _dbContext.Products.Update(product);
        }

        private async Task CreateProductAsync(UpdatedItem dto)
        {
            await _dbContext.Categories.AddAsync(new ProductCategory
            {
                Id = dto.UpdatedProductCategory.Id,
                Name = dto.UpdatedProductCategory.Name
            });
            await _dbContext.Products.AddAsync(new Product
            {
                ProductCategoryId = dto.UpdatedProductCategory.Id,
                Id = dto.ProductId,
                StockQty = dto.StockQty ?? 0,
                Name = dto.ProductName,
                Price = dto.Price ?? 0
            });
        }
    }
}