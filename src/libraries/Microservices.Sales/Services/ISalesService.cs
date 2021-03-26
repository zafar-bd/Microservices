using System.Threading.Tasks;
using Microservices.Common.Messages;

namespace Microservices.Sales.Services
{
    public interface ISalesService
    {
        Task<Data.Domains.Sales> SaveSalesAsync(SalesCommandReceived dto);
    }
}
