using System;

namespace Microservices.Order.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
