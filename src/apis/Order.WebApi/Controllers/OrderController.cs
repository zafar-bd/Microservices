using MassTransit;
using MediatR;
using Microservices.Common.Cache;
using Microservices.Common.Exceptions;
using Microservices.Common.Messages;
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
        private readonly IBus _publishEndpoint;

        public OrderController(
            IMediator mediator,
            ICacheHelper redisCacheClient,
            IBus publishEndpoint)
        {
            _mediator = mediator;
            _redisCacheClient = redisCacheClient;
            this._publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders([FromQuery] OrderQueryDto queryDto)
        {
            IEnumerable<OrderViewModel> orders = default;

            if (!queryDto.Cacheable)
            {
                orders = await _mediator.Send(queryDto);
                return Ok(orders);
            }

            var cacheKey = $"order-{queryDto.CustomerId}-{queryDto.OrderId}-{queryDto.DeliveredAt}-{queryDto.OrderedAt}-{queryDto.IsDelivered}-{queryDto.Cacheable}";
            orders = await _redisCacheClient.GetAsync<IEnumerable<OrderViewModel>>(cacheKey);

            if (orders != null)
            {
                Response.Headers.Add("X-DataSource", $"From-Cache");
                return Ok(orders);
            }

            orders = await _mediator.Send(queryDto);
            if (orders.Any())
                await _redisCacheClient.AddAsync(cacheKey, orders, 300);

            return Ok(orders);
        }

        [HttpGet("my")]
        [ProducesResponseType(typeof(IEnumerable<MyOrderViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMyOrders()
        {
            var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cacheKey = $"order-{customerId}";
            var ordersFromCache = await _redisCacheClient.GetAsync<IEnumerable<MyOrderViewModel>>(cacheKey);

            if (ordersFromCache != null)
            {
                Response.Headers.Add("X-DataSource", $"From-Cache");
                return Ok(ordersFromCache);
            }
            var ordersFromDb = await _mediator.Send(new MyOrderQueryDto { CustomerId = customerId });

            if (ordersFromDb.Any())
                await _redisCacheClient.AddAsync(cacheKey, ordersFromDb, 300);

            return Ok(ordersFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderReceived dto)
        {
            dto.CustomerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            dto.Email ??= User.FindFirstValue(ClaimTypes.Email);
            dto.Mobile ??= User.FindFirstValue("phone_number");

            var isValidStock = await _mediator.Send(new AvailableStockQueryDto { OrderReceivedItems = dto.OrderReceivedItems });

            if (!isValidStock)
                throw new BadRequestException("Sorry, Out of Stock!");

            await _publishEndpoint.Send(dto);
            await _redisCacheClient.RemoveAsync($"order-{ dto.CustomerId}");

            return Accepted();
        }
    }
}
