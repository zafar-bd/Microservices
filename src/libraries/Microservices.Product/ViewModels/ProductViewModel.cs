using System;

namespace Microservices.Product.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQty { get; set; }
        public int HoldQty { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
    }
}
