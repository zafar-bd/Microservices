using System;

namespace Ui.Blazor.WA.Models.Dto
{
    public class CartItemDto
    {
        public uint Qty { get; set; }
        public Guid ProductId { get; set; }
    }
}
