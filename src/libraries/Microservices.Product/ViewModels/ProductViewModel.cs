using System;

namespace Microservices.Product.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public uint StockQty { get; set; }
        public uint HoldQty { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
    }
}
