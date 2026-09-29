using MediatR;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;
using MultiShop.Order.Application.Features.Results.OrderDetailResults;
using MultiShop.Order.Application.Interfaces;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers;

public class GetOrderByUserIdQueryHandler : IRequestHandler<GetOrderByUserIdQuery, List<GetOrderByUserQueryResult>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByUserIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<GetOrderByUserQueryResult>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
    {
        var values = await _orderRepository.GetOrdersByUserId(request.Id);
        return values.Select(x=> new GetOrderByUserQueryResult
        {
            AddressId = x.AddressId,
            OrderStatus = x.OrderStatus,
            OrderingId = x.OrderingId,
            OrderDate = x.OrderDate,
            PaymentMethod = x.PaymentMethod,
            TotalPrice = x.TotalPrice,
            UserId = x.UserId,
            OrderDetails = x.OrderDetails.Select(detail => new GetOrderDetailQueryResult
            {
                OrderDetailId = detail.OrderDetailId,
                OrderingId = detail.OrderingId,
                ProductId = detail.ProductId,
                ProductName = detail.ProductName,
                ProductPrice = detail.ProductPrice,
                ProductAmount = detail.ProductAmount,
                ProductTotalPrice = detail.ProductTotalPrice
            }).ToList()
        }).ToList();
    }
}
