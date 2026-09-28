using MultiShop.Order.Application.Features.Queries.AddressQueries;
using MultiShop.Order.Application.Features.Results.AddressResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Handlers.AddressHandlers;

public class GetAddressByIdQueryHandler
{
    private readonly IRepository<Address> _addressRepository;

    public GetAddressByIdQueryHandler(IRepository<Address> addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<GetAddressByIdQueryResult> Handler(GetAddressByIdQuery query)
    {
        var values = await _addressRepository.GetByIdAsync(query.Id);
        return new GetAddressByIdQueryResult
        {
            AddressId = values.AddressId,
            UserId = values.UserId,
            Name = values.Name,
            Surname = values.Surname,
            Email = values.Email,
            Phone = values.Phone,
            Country = values.Country,
            District = values.District,
            City = values.City,
            Detail1 = values.Detail1,
            Detail2 = values.Detail2,
            Description = values.Description
        };
    }
}
