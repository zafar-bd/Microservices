using System;
using System.Collections.Generic;

namespace Microservices.Order.Data.Domains
{
    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        = new List<Product>();
    }
}
