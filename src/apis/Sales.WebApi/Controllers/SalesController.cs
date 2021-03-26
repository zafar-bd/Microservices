using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Microservices.Common.Cache;
using Microservices.Common.Exceptions;
using Microservices.Common.Messages;
using Microservices.Sales.Dtos;
using Microservices.Sales.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Sales.WebApi.Controllers
{
    [Route("api/v1/sales")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICacheHelper _redisCacheClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public SalesController(
            IMediator mediator,
            ICacheHelper redisCacheClient,
            IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _redisCacheClient = redisCacheClient;
            this._publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SalesViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders([FromQuery] SalesQueryDto queryDto)
        {
            var cacheKey = $"sales-{queryDto.CustomerId}-{queryDto.Reference}-{queryDto.SoldAt}-{queryDto.OperatedBy}";
            var salesFromCache = await _redisCacheClient.GetAsync<IEnumerable<SalesViewModel>>(cacheKey);

            if (salesFromCache != null)
            {
                Response.Headers.Add("X-DataSource", $"From-Cache");
                return Ok(salesFromCache);
            }
            var salesFromDb = await _mediator.Send(queryDto);

            if (salesFromDb.Any())
                await _redisCacheClient.AddAsync(cacheKey, salesFromDb, 300);

            return Ok(salesFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Sale([FromBody] SalesCommandReceived dto)
        {
            var isValidStock = await _mediator.Send(new AvailableStockQueryDto { SoldItems = dto.SoldItems });

            if (!isValidStock)
                throw new BadRequestException("Sorry, Out of Stock!");

            await _publishEndpoint.Publish(dto);
            await _redisCacheClient.RemoveAsync($"sales");

            return Accepted();
        }
    }
}
