using MultiShop.Order.WebApi.LoginServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Order.Application.Features.Commands.AddressCommands;
using MultiShop.Order.Application.Features.Handlers.AddressHandlers;
using MultiShop.Order.Application.Features.Queries.AddressQueries;

namespace MultiShop.Order.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly GetAddressQueryHandler _getAddressQueryHandler;
        private readonly GetAddressByIdQueryHandler _getAddressByIdQueryHandler;
        private readonly CreateAddressCommandHandler _createAddressCommandHandler;
        private readonly UpdateAddressCommandHandler _updateAddressCommandHandler;
        private readonly RemoveAddressCommandHandler _removeAddressCommandHandler;

        public AddressesController(
            GetAddressQueryHandler getAddressQueryHandler,
            GetAddressByIdQueryHandler getAddressByIdQueryHandler,
            RemoveAddressCommandHandler removeAddressCommandHandler,
            UpdateAddressCommandHandler updateAddressCommandHandler,
            CreateAddressCommandHandler createAddressCommandHandler,
            ILoginService loginService)
        {
            _getAddressQueryHandler = getAddressQueryHandler;
            _loginService = loginService;
            _getAddressByIdQueryHandler = getAddressByIdQueryHandler;
            _removeAddressCommandHandler = removeAddressCommandHandler;
            _updateAddressCommandHandler = updateAddressCommandHandler;
            _createAddressCommandHandler = createAddressCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _getAddressQueryHandler.Handler();
            if (User.IsInRole("Admin") || User.IsInRole("Manager"))
                return Ok(addresses);

            return Ok(addresses.Where(x => x.UserId == _loginService.GetUserId).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await _getAddressByIdQueryHandler.Handler(new GetAddressByIdQuery(id));
            if (value is null)
                return NotFound();
            if (!CanAccess(value.UserId))
                return Forbid();
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAddressCommand command)
        {
            command.UserId = _loginService.GetUserId;
            var addressId = await _createAddressCommandHandler.Handle(command);
            return Ok(addressId);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAddressCommand command)
        {
            var address = await _getAddressByIdQueryHandler.Handler(new GetAddressByIdQuery(command.AddressId));
            if (address is null)
                return NotFound();
            if (!CanAccess(address.UserId))
                return Forbid();
            command.UserId = address.UserId;
            await _updateAddressCommandHandler.Handle(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _getAddressByIdQueryHandler.Handler(new GetAddressByIdQuery(id));
            if (address is null)
                return NotFound();
            if (!CanAccess(address.UserId))
                return Forbid();
            await _removeAddressCommandHandler.Handle(new RemoveAddressCommand(id));
            return Ok();
        }

        private bool CanAccess(string userId)
        {
            return User.IsInRole("Admin") || User.IsInRole("Manager") || userId == _loginService.GetUserId;
        }
    }
}
