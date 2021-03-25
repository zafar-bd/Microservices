using System;
using System.Collections.Generic;

namespace Microservices.Common.Messages
{
    public class ProductUpdated
    {
        public List<UpdatedItem> UpdatedItems { get; set; }
        = new List<UpdatedItem>();
    }

    public class UpdatedItem
    {
        public Guid ProductId { get; set; }
        public uint? StockQty { get; set; }
        public uint? HoldQty { get; set; }
        public decimal? Price { get; set; }
        public string ProductName { get; set; }
        public UpdatedProductCategory UpdatedProductCategory { get; set; }
    }

    public class UpdatedProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}