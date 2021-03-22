using System;

namespace Microservices.Product.Data.Domains
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public uint StockQty { get; set; }
        public uint HoldQty { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
