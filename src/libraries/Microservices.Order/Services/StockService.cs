using Microservices.Common.Exceptions;
using Microservices.Common.Messages;
using Microservices.Order.Data.Context;
using Microservices.Order.Data.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Order.Services
{
    public class StockService : IStockService
    {
        private readonly OrderDbContext _dbContext;

        public StockService(OrderDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<IEnumerable<Product>> GetAvailableStockProductAsync(List<OrderReceivedItem> dto)
        {
            var products = await _dbContext
           .Products
           .Where(p => dto.Select(o => o.ProductId).Contains(p.Id))
           .ToListAsync();

            foreach (var product in products.OrderBy(p => p.Id))
            {
                foreach (var item in dto.OrderBy(p => p.ProductId))
                {
                    if (product.Id == item.ProductId && (product.StockQty - product.HoldQty) < item.Qty)
                    {
                        throw new BadRequestException("Out of Stock");
                    }
                }
            }

            return products;
        }
    }
}
