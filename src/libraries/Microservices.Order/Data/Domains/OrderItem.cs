using System;
namespace Microservices.Order.Data.Domains
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public uint Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
