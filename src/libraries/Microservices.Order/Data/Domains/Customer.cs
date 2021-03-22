﻿using System;
using System.Collections.Generic;

namespace Microservices.Order.Data.Domains
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public ICollection<Domains.Order> Orders { get; set; }
        = new List<Domains.Order>();
    }
}
