using System;
using System.Collections.Generic;

namespace Microservices.Sales.ViewModels
{
    public class SalesViewModel
    {
        public Guid Id { get; set; }
        public CustomerViewModel Customer { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public string OperatedBy { get; set; }
        public string Reference { get; set; }
        public DateTimeOffset? SoldAt { get; set; }
        public ICollection<SalesDetailsViewModel> SoldItems { get; set; }
        = new List<SalesDetailsViewModel>();
    }
}
