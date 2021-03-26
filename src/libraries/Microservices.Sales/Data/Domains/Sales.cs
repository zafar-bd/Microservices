using System;
using System.Collections.Generic;

namespace Microservices.Sales.Data.Domains
{
    public class Sales
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public string OperatedBy { get; set; }
        public string Reference { get; set; }
        public DateTimeOffset? SoldAt { get; set; }
        public ICollection<SalesDetails> SalesDetails { get; set; }
        = new List<SalesDetails>();
    }
}
