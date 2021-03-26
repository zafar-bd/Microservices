using System;

namespace Microservices.Sales.ViewModels
{
    public class SalesDetailsViewModel
    {
        public Guid Id { get; set; }
        public Guid SalesId { get; set; }
        public ProductViewModel Product { get; set; }
        public Guid ProductId { get; set; }
        public uint Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
