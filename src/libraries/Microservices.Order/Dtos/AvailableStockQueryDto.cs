using MediatR;
using Microservices.Common.Messages;
using System.Collections.Generic;

namespace Microservices.Order.Dtos
{
    public class AvailableStockQueryDto : IRequest<bool>
    {
        public List<OrderReceivedItem> OrderReceivedItems { get; set; }
        = new List<OrderReceivedItem>();
    }
}
