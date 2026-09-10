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

    public async Task<List<GetAddressByIdQueryResult>> Handler()
    {
        var values = await _addressRepository.GetAllAsync();
        return values.Select(x => new GetAddressByIdQueryResult
        {
            AddressId = x.AddressId,
            District = x.District,
            Detail = x.Detail,
            City = x.City,
            UserId = x.UserId
        }).ToList();
    }
}