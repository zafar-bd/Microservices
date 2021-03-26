using System;

namespace Microservices.Sales.Data.Domains
{
    public class SalesDetails
    {
        public Guid Id { get; set; }
        public Sales Sales { get; set; }
        public Guid SalesId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public uint Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
