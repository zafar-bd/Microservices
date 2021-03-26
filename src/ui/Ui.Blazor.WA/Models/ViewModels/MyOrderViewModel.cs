using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ui.Blazor.WA.Models.ViewModels
{
    public class MyOrderViewModel
    {
        public Guid Id { get; set; }
        public float AmountToPay { get; set; }
        public string ShipmentAddress { get; set; }
        public string DeliveredBy { get; set; }
        public string ReceivedBy { get; set; }
        public string DeliveredAt { get; set; }
        public bool IsDelivered  { get; set; }
        public DateTime OrderdAt { get; set; }
        public OrderItem[] OrderItems { get; set; }
    }

    public class OrderItem
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Productcategory ProductCategory { get; set; }
        public Guid ProductCategoryId { get; set; }
    }

    public class Productcategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

}
