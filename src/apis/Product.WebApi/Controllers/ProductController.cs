using MediatR;
using Microservices.Common.Cache;
using Microservices.Product.Dtos;
using Microservices.Product.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Product.WebApi.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICacheHelper _redisCacheClient;

        public ProductController(IMediator mediator, ICacheHelper redisCacheClient)
        {
            this._mediator = mediator;
            this._redisCacheClient = redisCacheClient;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryDto queryDto)
        {
            var cacheKey = $"product-{queryDto.ProductName}-{queryDto.CategoryId}";
            var productsFromCache = await _redisCacheClient.GetAsync<IEnumerable<ProductViewModel>>(cacheKey);

            if (productsFromCache != null)
            {
                Response.Headers.Add("X-DataSource", $"From-Cache");
                return Ok(productsFromCache);
            }
            var productsFromDb = await  _mediator.Send(queryDto);

            if (productsFromDb.Any())
                await _redisCacheClient.AddAsync(cacheKey, productsFromDb, 300);

            return Ok(productsFromDb);
        }
    }
}
