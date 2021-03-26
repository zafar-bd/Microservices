using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservices.Sales.Data.Context;
using Microservices.Sales.Dtos;
using Microservices.Sales.Services;
using Microservices.Sales.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Sales.Cqrs.Queries
{
    public class SalesQueriesHandler :
          IRequestHandler<SalesQueryDto, IEnumerable<SalesViewModel>>,
          IRequestHandler<AvailableStockQueryDto, bool>
    {
        private readonly SalesDbContext _orderDbContext;
        private readonly IMapper _mapper;
        private readonly IStockService _stockService;

        public SalesQueriesHandler(
            SalesDbContext orderDbContext,
            IMapper mapper,
            IStockService stockService)
        {
            _orderDbContext = orderDbContext;
            _mapper = mapper;
            this._stockService = stockService;
        }

        public async Task<IEnumerable<SalesViewModel>> Handle(SalesQueryDto request, CancellationToken cancellationToken)
        {
            var query = _orderDbContext.Sales.AsQueryable();

            if (request.CustomerId is not null)
                query = query.Where(p => p.CustomerId == request.CustomerId);

            if (request.Reference is not null)
                query = query.Where(p => p.Reference == request.Reference);

            if (request.OperatedBy is not null)
                query = query.Where(p => p.OperatedBy == request.OperatedBy);

            if (request.SoldAt is not null)
                query = query.Where(p => p.SoldAt >= request.SoldAt
                                      && p.SoldAt <= request.SoldAt);

            var sales = await query
                .ProjectTo<SalesViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);

            return sales;
        }

        public async Task<bool> Handle(AvailableStockQueryDto dto, CancellationToken cancellationToken)
        {
            var products = await _stockService.GetAvailableStockProductAsync(dto.SoldItems);
            return products.Any();
        }
    }
}
