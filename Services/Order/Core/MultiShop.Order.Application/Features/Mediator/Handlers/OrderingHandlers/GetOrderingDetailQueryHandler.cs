using MediatR;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;
using MultiShop.Order.Application.Features.Results.OrderDetailResults;
using MultiShop.Order.Application.Interfaces;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers;

public class GetOrderingDetailQueryHandler
    : IRequestHandler<GetOrderingDetailQuery, GetOrderingDetailQueryResult?>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderingDetailQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<GetOrderingDetailQueryResult?> Handle(GetOrderingDetailQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.Id);
        if (order is null)
            return null;

        return new GetOrderingDetailQueryResult
        {
            OrderingId = order.OrderingId,
            UserId = order.UserId,
            AddressId = order.AddressId,
            OrderDate = order.OrderDate,
            OrderStatus = order.OrderStatus,
            PaymentMethod = order.PaymentMethod,
            TotalPrice = order.TotalPrice,
            OrderDetails = order.OrderDetails.Select(item => new GetOrderDetailQueryResult
            {
                OrderDetailId = item.OrderDetailId,
                OrderingId = item.OrderingId,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductPrice = item.ProductPrice,
                ProductAmount = item.ProductAmount,
                ProductTotalPrice = item.ProductTotalPrice
            }).ToList()
        };
    }
}
