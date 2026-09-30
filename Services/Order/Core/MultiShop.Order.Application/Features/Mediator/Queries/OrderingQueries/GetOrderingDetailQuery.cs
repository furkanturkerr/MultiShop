using MediatR;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;

namespace MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;

public class GetOrderingDetailQuery(int id) : IRequest<GetOrderingDetailQueryResult?>
{
    public int Id { get; } = id;
}
