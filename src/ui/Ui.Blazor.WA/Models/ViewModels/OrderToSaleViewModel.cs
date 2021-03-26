using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ui.Blazor.WA.Models.ViewModels
{
    public class OrderToSaleViewModel
    {
        public Guid Id { get; set; }
        public CustomerViewModel Customer { get; set; }
        public Guid CustomerId { get; set; }
        public decimal AmountToPay { get; set; }
        public string ShipmentAddress { get; set; }
        public string DeliveredBy { get; set; }
        public string ReceivedBy { get; set; }
        public bool IsDelivered { get; set; }
        public DateTimeOffset? DeliveredAt { get; set; }
        public DateTimeOffset? OrderdAt { get; set; }
        public List<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
    
}
