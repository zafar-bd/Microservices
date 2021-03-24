using System;

namespace Ui.Blazor.WA.Models.Dto
{
    public class CartItemDto
    {
        public string ProductName { get; set; }
        public uint Qty { get; set; }
        public Guid ProductId { get; set; }
    }
}
