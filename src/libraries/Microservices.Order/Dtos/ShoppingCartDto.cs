using MediatR;
using System;
using System.Collections.Generic;

namespace Microservices.Order.Dtos
{
    public class ShoppingCartDto : INotification
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ShippingAddress { get; set; }
        public Guid CustomerId { get; set; }
        public List<CartItemDto> CartItemDtos { get; set; }
        = new List<CartItemDto>();
    }
}
