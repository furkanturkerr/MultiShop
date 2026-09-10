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

    public async Task Handle(CreateAddressCommand createAddressCommand)
    {
        await _addressRepository.CreateAsync(new Address
        {
            UserId = createAddressCommand.UserId,
            City = createAddressCommand.City,
            District = createAddressCommand.District,
            Detail = createAddressCommand.Detail,
        });
    }
}