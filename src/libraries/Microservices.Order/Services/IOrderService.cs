using Microservices.Common.Messages;
using System.Threading.Tasks;

namespace Microservices.Order.Services
{
    public interface IOrderService
    {
        Task<Data.Domains.Order> SaveOrderAsync(OrderReceived dto);
    }
}
