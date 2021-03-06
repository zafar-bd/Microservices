using System;

namespace Ui.Blazor.WA.Models.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQty { get; set; }
        public int HoldQty { get; set; }
        public CategoryViewModel ProductCategory { get; set; }
    }
}
