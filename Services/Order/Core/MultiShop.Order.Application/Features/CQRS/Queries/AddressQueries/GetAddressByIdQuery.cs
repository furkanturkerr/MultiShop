namespace MultiShop.Order.Application.Features.Queries.AddressQueries;

public class GetAddressByIdQuery
{
    public int Id { get; set; }

    public GetAddressByIdQuery(int id)
    {
        Id = id;
    }
    
}