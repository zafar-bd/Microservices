using System;
using System.Collections.Generic;

namespace Microservices.Common.Messages
{
    public class SalesCommandReceived
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public string OperatedBy { get; set; }
        public string Reference { get; set; }
        public List<SoldItem> SoldItems { get; set; }
        = new List<SoldItem>();
    }

    public class SoldItem
    {
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public Guid ProductId { get; set; }
    }
}
