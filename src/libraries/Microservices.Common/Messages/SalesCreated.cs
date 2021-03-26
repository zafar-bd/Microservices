using System;
using System.Collections.Generic;

namespace Microservices.Common.Messages
{
    public class SalesCreated
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ShippingAddress { get; set; }
        public decimal AmountToPay { get; set; }
        public Guid CustomerId { get; set; }
        public List<SalesItemsCreated> SalesItemsCreated { get; set; }
        = new List<SalesItemsCreated>();
    }

    public class SalesItemsCreated
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public uint Qty { get; set; }
        public decimal Price { get; set; }
    }
}
