using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MediatR;
using Microservices.Common.Exceptions;
using Microservices.Common.Messages;
using Microservices.Order.Data.Context;
using Microservices.Product.Data.Domains;
using Microservices.Product.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Product.Cqrs.Commands
{
    public class ProductCommandHandler
        : IRequestHandler<ProductCreateCommandDto>,
          IRequestHandler<ProductUpdateCommandDto>
    {
        private readonly ProductDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductCommandHandler(
            ProductDbContext dbContext,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Unit> Handle(ProductUpdateCommandDto dto, CancellationToken cancellationToken)
        {
            var productFromDb = await _dbContext
                .Products
                .Where(p => p.Id == dto.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (productFromDb is null)
                throw new BadRequestException("Not found");

            if (productFromDb.ProductCategory is not null)
            {
                productFromDb.ProductCategory.Name = dto.CategoryName;
            }
            else
            {
                await _dbContext.Categories.AddAsync(new ProductCategory
                {
                    Name = dto.CategoryName
                }, cancellationToken);
            }

            productFromDb.Name = dto.Name;
            productFromDb.HoldQty = dto.HoldQty;
            productFromDb.StockQty = dto.StockQty;
            productFromDb.Price = dto.Price;
            _dbContext.Products.Update(productFromDb);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await this.ProductUpdatedEvent(dto);
            return default;
        }

        public async Task<Unit> Handle(ProductCreateCommandDto dto, CancellationToken cancellationToken)
        {
            var categoryFromDb = await _dbContext
                .Categories
                .Where(p => p.Id == dto.ProductCategoryId)
                .FirstOrDefaultAsync(cancellationToken);

            var product = new Data.Domains.Product
            {
                Name = dto.Name,
                HoldQty = dto.HoldQty,
                Price = dto.Price,
                StockQty = dto.StockQty
            };

            if (categoryFromDb is not null)
            {
                categoryFromDb.Name = dto.CategoryName;
            }
            else
            {
                product.ProductCategory = new ProductCategory
                {
                    Name = dto.CategoryName
                };
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            dto.Id = product.Id;
            dto.ProductCategoryId = product.ProductCategoryId;
            await this.ProductUpdatedEvent(dto);
            return default;
        }

        async Task ProductUpdatedEvent(ProductCreateCommandDto dto)
        {
            var productUpdated = new ProductUpdated
            {
                ProductUpdatedFrom = ProductUpdatedFrom.ProductService
            };
            productUpdated.UpdatedItems.Add(new UpdatedItem
            {
                StockQty = dto.StockQty,
                HoldQty = dto.HoldQty,
                ProductId = (Guid)dto.Id,
                Price = dto.Price,
                ProductName = dto.Name,
                UpdatedProductCategory = new UpdatedProductCategory
                {
                    Name = dto.CategoryName,
                    Id = (Guid)dto.ProductCategoryId
                }
            });

            await _publishEndpoint.Publish(productUpdated);
        }

        async Task ProductUpdatedEvent(ProductUpdateCommandDto dto)
        {
            var productUpdated = new ProductUpdated
            {
                ProductUpdatedFrom = ProductUpdatedFrom.ProductService
            };
            productUpdated.UpdatedItems.Add(new UpdatedItem
            {
                StockQty = dto.StockQty,
                HoldQty = dto.HoldQty,
                ProductId = (Guid)dto.Id,
                Price = dto.Price,
                ProductName = dto.Name,
                UpdatedProductCategory = new UpdatedProductCategory
                {
                    Name = dto.CategoryName,
                    Id = (Guid)dto.ProductCategoryId
                }
            });
            await _publishEndpoint.Publish(productUpdated);
        }
    }
}
