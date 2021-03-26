using System;
namespace Microservices.Order.ViewModels
{
    public class OrderItemViewModel
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public ProductViewModel Product { get; set; }
        public Guid ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
