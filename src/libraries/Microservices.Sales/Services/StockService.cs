using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Common.Exceptions;
using Microservices.Common.Messages;
using Microservices.Sales.Data.Context;
using Microservices.Sales.Data.Domains;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Sales.Services
{
    public class StockService : IStockService
    {
        private readonly SalesDbContext _dbContext;

        public StockService(SalesDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<IEnumerable<Product>> GetAvailableStockProductAsync(List<SoldItem> dto)
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
