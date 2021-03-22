using MediatR;
using Microservices.Common.Cache;
using Microservices.Order.Dtos;
using Microservices.Order.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Order.WebApi.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICacheHelper _redisCacheClient;

        public OrderController(IMediator mediator, ICacheHelper redisCacheClient)
        {
            _mediator = mediator;
            _redisCacheClient = redisCacheClient;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders([FromQuery] OrderQueryDto queryDto)
        {
            var cacheKey = $"order-{queryDto.CustomerId}-{queryDto.OrderId}-{queryDto.DeliveredAt}-{queryDto.OrderedAt}";
            var ordersFromCache = await _redisCacheClient.GetAsync<IEnumerable<OrderViewModel>>(cacheKey);

            if (ordersFromCache != null)
            {
                Response.Headers.Add("X-DataSource", $"From-Cache");
                return Ok(ordersFromCache);
            }
            var ordersFromDb = await _mediator.Send(queryDto);

            if (ordersFromDb.Any())
                await _redisCacheClient.AddAsync(cacheKey, ordersFromDb, 300);

            return Ok(ordersFromDb);
        }

        [HttpGet("my")]
        [ProducesResponseType(typeof(IEnumerable<MyOrderViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMyOrders()
        {
            var customerId = Guid.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var cacheKey = $"order-{customerId}";
            var ordersFromCache = await _redisCacheClient.GetAsync<IEnumerable<MyOrderViewModel>>(cacheKey);

            if (ordersFromCache != null)
            {
                Response.Headers.Add("X-DataSource", $"From-Cache");
                return Ok(ordersFromCache);
            }
            var ordersFromDb = await _mediator.Send(new MyQueryDto { CustomerId = customerId });

            if (ordersFromDb.Any())
                await _redisCacheClient.AddAsync(cacheKey, ordersFromDb, 300);

            return Ok(ordersFromDb);
        }
    }
}
