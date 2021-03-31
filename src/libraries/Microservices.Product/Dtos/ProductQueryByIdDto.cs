using MediatR;
using Microservices.Product.ViewModels;
using System;

namespace Microservices.Product.Dtos
{
    public class ProductQueryByIdDto : IRequest<ProductViewModel>
    {
        public Guid Id { get; set; }
    }
}
