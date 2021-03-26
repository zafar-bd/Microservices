using System;
using System.Collections.Generic;

namespace Microservices.Sales.Data.Domains
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQty { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public Guid ProductCategoryId { get; set; }
        public ICollection<SalesDetails> SoldItems { get; set; }
        = new List<SalesDetails>();
    }
}
