using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Common.Messages;
using Microservices.Sales.Data.Domains;

namespace Microservices.Sales.Services
{
   public interface IStockService
    {
        Task<IEnumerable<Product>> GetAvailableStockProductAsync(List<SoldItem> dto);
    }
}
