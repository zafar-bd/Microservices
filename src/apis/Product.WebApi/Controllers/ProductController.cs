using MediatR;
using Microservices.Product.Dtos;
using Microservices.Product.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.WebApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        //[ResponseType(typeof(List<ProductViewModel>)]
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryDto queryDto)
            => Ok(await _mediator.Send(queryDto));
    }
}
