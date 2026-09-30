using MultiShop.Order.WebApi.Services;
using MultiShop.Order.Application.Features.Handlers.AddressHandlers;
using MultiShop.Order.Application.Features.Queries.AddressQueries;
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
        private readonly GetAddressByIdQueryHandler _addresses;
        private readonly CheckoutService _checkout;
        private readonly ILoginService _loginService;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        private readonly ILogger<OrderingsController> _logger;

        public OrderingsController(
            IMediator mediator,
            ILoginService loginService,
            IRabbitMqPublisher rabbitMqPublisher,
            ILogger<OrderingsController> logger,
            GetAddressByIdQueryHandler addresses,
            CheckoutService checkout)
        {
            _mediator = mediator;
            _addresses = addresses;
            _checkout = checkout;
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

        [HttpGet("admin/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOrderDetail(int id)
        {
            var order = await _mediator.Send(new GetOrderingDetailQuery(id));
            return order is null ? NotFound() : Ok(order);
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
            var value = await _mediator.Send(new GetOrderingDetailQuery(id));

            if (value is null)
                return NotFound();
            if (!User.IsInRole("Admin") && value.UserId != _loginService.GetUserId)
                return Forbid();

            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderingCommand command)
        {
            command.UserId = _loginService.GetUserId;
            var address = await _addresses.Handler(new GetAddressByIdQuery(command.AddressId));
            if (address is null)
                return BadRequest("Adres bulunamadı.");
            if (address.UserId != command.UserId)
                return Forbid();
            if (command.PaymentMethod != "Kredi/Banka Kartı")
                return BadRequest("Ödeme yöntemi geçersiz.");

            try
            {
                await _checkout.PrepareAsync(command);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "Sepet servisine ulaşılamıyor.");
            }

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
