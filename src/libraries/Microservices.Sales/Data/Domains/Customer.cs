using System;
using System.Collections.Generic;

namespace Microservices.Sales.Data.Domains
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public ICollection<Domains.Sales> Sales { get; set; }
        = new List<Domains.Sales>();
    }
}
