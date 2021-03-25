using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ui.Blazor.WA.Models.ViewModels
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public float AmountToPay { get; set; }
        public object ShipmentAddress { get; set; }
        public object DeliveredBy { get; set; }
        public object ReceivedBy { get; set; }
        public object DeliveredAt { get; set; }
        public DateTime OrderdAt { get; set; }
        public Orderitem[] OrderItems { get; set; }
    }

    public class Orderitem
    {
        public string id { get; set; }
        public string orderId { get; set; }
        public Product product { get; set; }
        public string productId { get; set; }
        public int qty { get; set; }
        public float price { get; set; }
        public float discount { get; set; }
    }

    public class Product
    {
        public string id { get; set; }
        public string name { get; set; }
        public Productcategory productCategory { get; set; }
        public string productCategoryId { get; set; }
    }

    public class Productcategory
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}
