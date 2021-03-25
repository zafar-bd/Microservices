using MediatR;
using Microservices.Product.ViewModels;
using System;
using System.Collections.Generic;

namespace Microservices.Product.Dtos
{
    public class ProductQueryDto : IRequest<IEnumerable<ProductViewModel>>
    {
        public string ProductName { get; set; }
        public Guid? CategoryId { get; set; }
        public bool Cacheable { get; set; } = true;
    }
}
