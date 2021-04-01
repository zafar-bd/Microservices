using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservices.Order.Data.Context;
using Microservices.Product.Dtos;
using Microservices.Product.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microservices.Common.Exceptions;

namespace Microservices.Product.Cqrs.Queries
{
    public class ProductQueriesHandler :
          IRequestHandler<ProductQueryDto, IEnumerable<ProductViewModel>>,
          IRequestHandler<ProductQueryByIdDto, ProductViewModel>
    {
        private readonly ProductDbContext _productDbContext;
        private readonly IMapper _mapper;

        public ProductQueriesHandler(ProductDbContext productDbContext,
            IMapper mapper)
        {
            this._productDbContext = productDbContext;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<ProductViewModel>> Handle(ProductQueryDto request, CancellationToken cancellationToken)
        {
            var query = _productDbContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(request.ProductName))
                query = query.Where(p => p.Name.StartsWith(request.ProductName));

            if (request.CategoryId is not null)
                query = query.Where(p => p.ProductCategoryId == request.CategoryId);

            var products = await query
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return products;
        }

        public async Task<ProductViewModel> Handle(ProductQueryByIdDto request, CancellationToken cancellationToken)
        {
            var product = await _productDbContext
                .Products
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product is null)
                throw new BadRequestException("Not found");

            return product;
        }
    }
}
