using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microservices.Common.Messages;
using Microservices.Order.Data.Context;
using Microservices.Order.Data.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace OrderService.ProductSync.Worker
{
    public class ProductConsumer : IConsumer<ProductUpdated>
    {
        ILogger<ProductConsumer> _logger;
        private readonly OrderDbContext _dbContext;

        public ProductConsumer(ILogger<ProductConsumer> logger, OrderDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ProductUpdated> context)
        {
            await this.HandleProductsAsync(context.Message);
            _logger.LogInformation("Products for Order Service has been consumed");
        }

        private async Task HandleProductsAsync(ProductUpdated message)
        {
            var productsFromDb = await _dbContext.Products
                .Where(p => message.UpdatedItems.Select(i => i.ProductId).Contains(p.Id))
                .Include(p => p.ProductCategory)
                .ToListAsync();

            await HandleProductCategoryAsync(message, productsFromDb);
            await HandleProductAsync(message, productsFromDb);
            await _dbContext.SaveChangesAsync();
        }

        private async Task HandleProductAsync(ProductUpdated message, List<Product> productsFromDb)
        {
            foreach (var product in productsFromDb.OrderBy(p => p.Id))
            {
                foreach (var dto in message.UpdatedItems.OrderBy(p => p.ProductId))
                {
                    if (product.Id != dto.ProductId)
                        await this.CreateProductAsync(dto);
                    else
                        this.UpdateProduct(message.ProductUpdatedFrom, dto, product);
                }
            }
        }

        private async Task HandleProductCategoryAsync(ProductUpdated message, List<Product> productsFromDb)
        {
            foreach (var product in productsFromDb.OrderBy(p => p.Id))
            {
                foreach (var dto in message.UpdatedItems.OrderBy(p => p.ProductId))
                {
                    if (product.ProductCategoryId != dto.UpdatedProductCategory.Id)
                    {
                        await _dbContext.Categories.AddAsync(new ProductCategory
                        {
                            Id = dto.UpdatedProductCategory.Id,
                            Name = dto.UpdatedProductCategory.Name
                        });
                    }
                    else
                    {
                        product.ProductCategory.Name = dto.UpdatedProductCategory.Name;
                        _dbContext.Categories.Update(product.ProductCategory);
                    }
                }
            }
        }

        private void UpdateProduct(ProductUpdatedFrom updatedFrom, UpdatedItem dto, Product product)
        {
            if (!string.IsNullOrEmpty(dto.ProductName))
                product.Name = dto.ProductName;
            if (updatedFrom != ProductUpdatedFrom.OrderService)
            {
                if (dto.HoldQty is not null)
                    product.HoldQty += (int)dto.HoldQty;
                if (dto.StockQty is not null)
                    product.StockQty += (int)dto.StockQty;
            }
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
                HoldQty = dto.HoldQty ?? 0,
                StockQty = dto.StockQty ?? 0,
                Name = dto.ProductName,
                Price = dto.Price ?? 0
            });
        }
    }
}
