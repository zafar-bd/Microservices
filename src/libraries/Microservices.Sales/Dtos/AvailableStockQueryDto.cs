using System.Collections.Generic;
using MediatR;
using Microservices.Common.Messages;

namespace Microservices.Sales.Dtos
{
    public class AvailableStockQueryDto : IRequest<bool>
    {
        public List<SoldItem> SoldItems { get; set; }
        = new List<SoldItem>();
    }
}
