using System.Collections.Generic;

namespace Ui.Blazor.WA.Models.Dto
{
    public class ShoppingCartDto
    {
        public string ShippingAddress { get; set; }
        public bool CommandStarted { get; set; }
        public List<CartItemDto> OrderReceivedItems { get; set; }
    }
}
