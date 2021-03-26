using System;
using System.Collections.Generic;

namespace Microservices.Common.Messages
{
    public class OrderCreated
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ShippingAddress { get; set; }
        public decimal AmountToPay { get; set; }
        public Guid CustomerId { get; set; }
        public List<OrderItemsCreated> OrderItemsCreated { get; set; }
        = new List<OrderItemsCreated>();
    }

    public class OrderItemsCreated
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}
