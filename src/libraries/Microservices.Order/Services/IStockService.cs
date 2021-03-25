using Microservices.Common.Messages;
using Microservices.Order.Data.Domains;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Order.Services
{
   public interface IStockService
    {
        Task<IEnumerable<Product>> GetAvailableStockProductAsync(List<OrderReceivedItem> dto);
    }
}
