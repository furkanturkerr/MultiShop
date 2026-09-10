using MultiShop.Order.Application.Features.Queries.OrderDetailQueries;
using MultiShop.Order.Application.Features.Results.OrderDetailResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Handlers.OrderDetailHandlers;

public class GetOrderDetailByIdQueryHandler
{
    private readonly IRepository<OrderDetail> _orderDetailRepository;

    public GetOrderDetailByIdQueryHandler(IRepository<OrderDetail> orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task<GetOrderDetailByIdQueryResult> Handler(GetOrderDetailByIdQuery query)
    {
        var values = await _orderDetailRepository.GetByIdAsync(query.Id);
        return new GetOrderDetailByIdQueryResult
        {
            OrderingId = values.OrderingId,
            OrderDetailId = values.OrderDetailId,
            ProductAmount = values.ProductAmount,
            ProductName = values.ProductName,
            ProductPrice = values.ProductPrice,
            ProductId = values.ProductId,
            ProductTotalPrice = values.ProductTotalPrice
        };
    }
}