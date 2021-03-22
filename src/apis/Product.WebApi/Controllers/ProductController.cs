using MediatR;
using Microservices.Product.Dtos;
using Microservices.Product.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Product.WebApi.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryDto queryDto)
        => Ok(await _mediator.Send(queryDto));
    }
}
