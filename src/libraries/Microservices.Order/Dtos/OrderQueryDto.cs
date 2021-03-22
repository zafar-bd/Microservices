using MediatR;
using Microservices.Order.ViewModels;
using System;
using System.Collections.Generic;

namespace Microservices.Order.Dtos
{
    public class OrderQueryDto : IRequest<IEnumerable<OrderViewModel>>
    {
        public DateTime? OrderedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
