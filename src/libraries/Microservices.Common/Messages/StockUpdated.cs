using System;

namespace Microservices.Common.Messages
{
    public class StockUpdated
    {
        public Guid ProductId { get; set; }
        public uint StockQty { get; set; }
        public uint HoldQty { get; set; }
    }
}
