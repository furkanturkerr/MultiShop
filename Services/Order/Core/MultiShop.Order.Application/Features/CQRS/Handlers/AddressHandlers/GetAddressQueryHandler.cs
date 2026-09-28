using MultiShop.Order.Application.Features.Results.AddressResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Handlers.AddressHandlers;

public class GetAddressQueryHandler
{
    private readonly IRepository<Address> _addressRepository;

    public GetAddressQueryHandler(IRepository<Address> addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<List<GetAddressQueryResult>> Handler()
    {
        var values = await _addressRepository.GetAllAsync();
        return values.Select(x => new GetAddressQueryResult
        {
            AddressId = x.AddressId,
            UserId = x.UserId,
            Name = x.Name,
            Surname = x.Surname,
            Email = x.Email,
            Phone = x.Phone,
            Country = x.Country,
            District = x.District,
            City = x.City,
            Detail1 = x.Detail1,
            Detail2 = x.Detail2,
            Description = x.Description
        }).ToList();
    }
}
