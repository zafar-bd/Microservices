using MediatR;
using Microservices.Order.ViewModels;
using System;
using System.Collections.Generic;

namespace Microservices.Order.Dtos
{
    public class MyOrderQueryDto : IRequest<IEnumerable<MyOrderViewModel>>
    {
        public Guid? CustomerId { get; set; }
    }
}
