using MediatR;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;

namespace MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;

public class GetOrderByUserIdQuery : IRequest<List<GetOrderByUserQueryResult>>
{
    public string Id { get; set; }
    public GetOrderByUserIdQuery(string id)
    {
        Id = id;
    }
}