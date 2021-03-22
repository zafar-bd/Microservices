using MediatR;
using Microservices.Order.ViewModels;
using System;
using System.Collections.Generic;

namespace Microservices.Order.Dtos
{
    public class MyQueryDto : IRequest<IEnumerable<OrderViewModel>>
    {
        public Guid? CustomerId { get; set; }
    }
}
