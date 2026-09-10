using MultiShop.Order.Application.Features.Commands.AddressCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Handlers.AddressHandlers;

public class UpdateAddressCommandHandler
{
    private readonly IRepository<Address> _addressRepository;

    public UpdateAddressCommandHandler(IRepository<Address> addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task Handle(UpdateAddressCommand updateAddressCommand)
    {
        var value = await _addressRepository.GetByIdAsync(updateAddressCommand.AddressId);
        value.City = updateAddressCommand.City;
        value.District = updateAddressCommand.District;
        value.Detail = updateAddressCommand.Detail;
        await _addressRepository.UpdateAsync(value);
    }
}