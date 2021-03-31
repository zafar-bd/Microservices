using System;
using MediatR;

namespace Microservices.Product.Dtos
{
    public class ProductUpdateCommandDto : IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQty { get; set; }
        public int HoldQty { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
