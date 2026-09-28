using MultiShop.Order.Application.Features.Commands.AddressCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Handlers.AddressHandlers;

public class CreateAddressCommandHandler
{
    private readonly IRepository<Address> _addressRepository;

    public CreateAddressCommandHandler(IRepository<Address> addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<int> Handle(CreateAddressCommand createAddressCommand)
    {
        var address = new Address
        {
            UserId = createAddressCommand.UserId,
            Name = createAddressCommand.Name,
            Surname = createAddressCommand.Surname,
            Email = createAddressCommand.Email,
            Phone = createAddressCommand.Phone,
            Country = createAddressCommand.Country,
            District = createAddressCommand.District,
            City = createAddressCommand.City,
            Detail1 = createAddressCommand.Detail1,
            Detail2 = createAddressCommand.Detail2,
            Description = createAddressCommand.Description
        };

        await _addressRepository.CreateAsync(address);
        return address.AddressId;
    }
}
