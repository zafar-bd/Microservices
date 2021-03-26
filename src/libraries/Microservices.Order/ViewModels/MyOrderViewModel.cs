using System;
using System.Collections.Generic;

namespace Microservices.Order.ViewModels
{
    public class MyOrderViewModel
    {
        public Guid Id { get; set; }
        public decimal AmountToPay { get; set; }
        public string ShipmentAddress { get; set; }
        public string DeliveredBy { get; set; }
        public string ReceivedBy { get; set; }
        public bool IsDelivered { get; set; }
        public DateTimeOffset? DeliveredAt { get; set; }
        public DateTimeOffset? OrderdAt { get; set; }
        public ICollection<OrderItemViewModel> OrderItems { get; set; }
        = new List<OrderItemViewModel>();
    }
}
