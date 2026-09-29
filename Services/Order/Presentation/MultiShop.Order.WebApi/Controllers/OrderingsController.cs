using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.MessageBus;
using MultiShop.MessageBus.Events;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.Order.WebApi.LoginServices;

namespace MultiShop.Order.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoginService _loginService;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        private readonly ILogger<OrderingsController> _logger;

        public OrderingsController(
            IMediator mediator,
            ILoginService loginService,
            IRabbitMqPublisher rabbitMqPublisher,
            ILogger<OrderingsController> logger)
        {
            _mediator = mediator;
            _loginService = loginService;
            _rabbitMqPublisher = rabbitMqPublisher;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OrderList()
        {
            var value = await _mediator.Send(new GetOrderingQuery());
            return Ok(value);
        }

        [HttpGet("my")]
        public async Task<IActionResult> MyOrders()
        {
            var values = await _mediator.Send(
                new GetOrderByUserIdQuery(_loginService.GetUserId));

            return Ok(values);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> OrderById(int id)
        {
            var value = await _mediator.Send(new GetOrderingByIdQuery(id));

            if (!User.IsInRole("Admin") && value.UserId != _loginService.GetUserId)
                return Forbid();

            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderingCommand command)
        {
            command.UserId = _loginService.GetUserId;
            var orderingId = await _mediator.Send(command);

            try
            {
                await _rabbitMqPublisher.PublishOrderCreatedAsync(
                    new OrderCreatedEvent(
                        orderingId,
                        command.UserId,
                        command.TotalPrice,
                        command.PaymentMethod,
                        DateTime.UtcNow),
                    HttpContext.RequestAborted);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Sipariş {OrderId} oluşturuldu ancak RabbitMQ mesajı gönderilemedi.",
                    orderingId);
            }

            return Ok(orderingId);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateOrderingCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            await _mediator.Send(new RemoveOrderingCommand(id));
            return Ok();
        }
    }
}
