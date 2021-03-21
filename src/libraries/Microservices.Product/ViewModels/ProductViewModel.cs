using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Product.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
