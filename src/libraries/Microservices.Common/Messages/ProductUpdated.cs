using System;
using System.Collections.Generic;

namespace Microservices.Common.Messages
{
    public class ProductUpdated
    {
        public ProductUpdatedFrom ProductUpdatedFrom { get; set; }
        public List<UpdatedItem> UpdatedItems { get; set; }
        = new List<UpdatedItem>();
    }

    public class UpdatedItem
    {
        public Guid ProductId { get; set; }
        public int? StockQty { get; set; }
        public int? HoldQty { get; set; }
        public decimal? Price { get; set; }
        public string ProductName { get; set; }
        public UpdatedProductCategory UpdatedProductCategory { get; set; }
    }

    public class UpdatedProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public enum ProductUpdatedFrom
    {
        ProductService,
        OrderService,
        SalesService
    }
}