using System;
using System.Collections.Generic;
using MediatR;
using Microservices.Sales.ViewModels;

namespace Microservices.Sales.Dtos
{
    public class SalesQueryDto : IRequest<IEnumerable<SalesViewModel>>
    {
        public string OperatedBy { get; set; }
        public string Reference { get; set; }
        public DateTimeOffset? SoldAt { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
