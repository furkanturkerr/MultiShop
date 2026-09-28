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
        value.UserId = updateAddressCommand.UserId;
        value.Name = updateAddressCommand.Name;
        value.Surname = updateAddressCommand.Surname;
        value.Email = updateAddressCommand.Email;
        value.Phone = updateAddressCommand.Phone;
        value.Country = updateAddressCommand.Country;
        value.District = updateAddressCommand.District;
        value.City = updateAddressCommand.City;
        value.Detail1 = updateAddressCommand.Detail1;
        value.Detail2 = updateAddressCommand.Detail2;
        value.Description = updateAddressCommand.Description;
        await _addressRepository.UpdateAsync(value);
    }
}
