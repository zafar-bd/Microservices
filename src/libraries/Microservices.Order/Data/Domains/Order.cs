using System;
using System.Collections.Generic;

namespace Microservices.Order.Data.Domains
{
    public class Order
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
        public decimal AmountToPay { get; set; }
        public string ShipmentAddress { get; set; }
        public string DeliveredBy { get; set; }
        public string ReceivedBy { get; set; }
        public bool IsDelivered { get; set; }
        public DateTimeOffset? DeliveredAt { get; set; }
        public DateTimeOffset? OrderdAt { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();
    }
}
