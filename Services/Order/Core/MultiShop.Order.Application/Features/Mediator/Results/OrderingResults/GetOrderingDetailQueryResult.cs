using MultiShop.Order.Application.Features.Results.OrderDetailResults;

namespace MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;

public class GetOrderingDetailQueryResult : GetOrderingQueryResult
{
    public List<GetOrderDetailQueryResult> OrderDetails { get; set; } = [];
}
