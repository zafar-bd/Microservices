using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Common.Messages
{
   public class OrderStatusUpdated
    {
        public bool IsDelivered { get; set; }
        public Guid OrderId { get; set; }
        public Guid? SalesId { get; set; }
    }
}
