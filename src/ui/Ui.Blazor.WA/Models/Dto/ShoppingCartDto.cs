using System.Collections.Generic;

namespace Ui.Blazor.WA.Models.Dto
{
    public class ShoppingCartDto
    {
        public string ShippingAddress { get; set; }
        public List<CartItemDto> CartItemDtos { get; set; }
    }
}
