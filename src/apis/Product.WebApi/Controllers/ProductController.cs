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
            IEnumerable<ProductViewModel> products = default;
            if (!queryDto.Cacheable)
            {
                products = await _mediator.Send(queryDto);
                return Ok(products);
            }

            var cacheKey = $"product-{queryDto.ProductName}-{queryDto.CategoryId}";

            products = await _redisCacheClient.GetAsync<IEnumerable<ProductViewModel>>(cacheKey);

            if (products != null)
            {
                Response.Headers.Add("X-DataSource", $"From-Cache");
                return Ok(products);
            }

            if (products.Any())
                await _redisCacheClient.AddAsync(cacheKey, products, 300);

            return Ok(products);
        }
    }
}
