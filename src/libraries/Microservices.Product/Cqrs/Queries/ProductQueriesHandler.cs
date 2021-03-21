using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservices.Order.Data.Context;
using Microservices.Product.Dtos;
using Microservices.Product.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Product.Cqrs.Queries
{
    public class ProductQueriesHandler :
          IRequestHandler<ProductQueryDto, IEnumerable<ProductViewModel>>
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
            
            if (string.IsNullOrEmpty(request.ProductName))
                query = query.Where(p => p.Name.StartsWith(request.ProductName));
            
            if (request.CategoryId != Guid.Empty)
                query = query.Where(p => p.ProductCategoryId == request.CategoryId);

            var products = await query
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return products;
        }
    }
}
