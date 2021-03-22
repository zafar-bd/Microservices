﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservices.Order.Data.Context;
using Microservices.Order.Dtos;
using Microservices.Order.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Order.Cqrs.Queries
{
    public class OrderQueriesHandler :
          IRequestHandler<OrderQueryDto, IEnumerable<OrderViewModel>>,
          IRequestHandler<MyQueryDto, IEnumerable<OrderViewModel>>
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly IMapper _mapper;

        public OrderQueriesHandler(OrderDbContext orderDbContext,
            IMapper mapper)
        {
            _orderDbContext = orderDbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<OrderViewModel>> Handle(MyQueryDto request, CancellationToken cancellationToken)
        {
            var orders = await _orderDbContext
                .Orders
                .Where(p => p.CustomerId == request.CustomerId)
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrderViewModel>> Handle(OrderQueryDto request, CancellationToken cancellationToken)
        {
            var query = _orderDbContext.Orders.AsQueryable();

            if (request.CustomerId is not null)
                query = query.Where(p => p.CustomerId == request.CustomerId);

            if (request.OrderId is not null)
                query = query.Where(p => p.Id == request.OrderId);

            if (request.OrderedAt is not null)
                query = query.Where(p => p.OrderdAt >= request.OrderedAt
                                      && p.OrderdAt <= request.OrderedAt);

            if (request.DeliveredAt is not null)
                query = query.Where(p => p.DeliveredAt >= request.DeliveredAt
                                      && p.DeliveredAt <= request.DeliveredAt);

            var orders = await query
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return orders;
        }
    }
}