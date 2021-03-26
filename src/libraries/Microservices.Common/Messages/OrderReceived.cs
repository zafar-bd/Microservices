using System;
using System.Collections.Generic;

namespace Microservices.Common.Messages
{
    public class OrderReceived
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ShippingAddress { get; set; }
        public Guid CustomerId { get; set; }
        public List<OrderReceivedItem> OrderReceivedItems { get; set; }
        = new List<OrderReceivedItem>();
    }

    public class OrderReceivedItem
    {
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public Guid ProductId { get; set; }
    }
}
