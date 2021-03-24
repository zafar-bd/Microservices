using System;

namespace Microservices.Order.Dtos
{
    public class CartItemDto
    {
        public uint Qty { get; set; }
        public Guid ProductId { get; set; }
    }
}
