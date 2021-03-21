using MediatR;
using Microservices.Product.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Product.Dtos
{
    public class ProductQueryDto : IRequest<List<ProductViewModel>>
    {
        public string ProductName { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
