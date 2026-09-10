using MultiShop.Order.Application.Features.Results.OrderDetailResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Handlers.OrderDetailHandlers;

public class GetOrderDetailQueryHandler
{
    private readonly IRepository<OrderDetail> _orderDetailRepository;

    public GetOrderDetailQueryHandler(IRepository<OrderDetail> orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task<List<GetOrderDetailQueryResult>> Handler()
    {
        var values = await _orderDetailRepository.GetAllAsync();
        return values.Select(x=> new GetOrderDetailQueryResult
        {
            OrderingId = x.OrderingId,
            OrderDetailId = x.OrderDetailId,
            ProductAmount = x.ProductAmount,
            ProductName = x.ProductName,
            ProductPrice = x.ProductPrice,
            ProductId = x.ProductId,
            ProductTotalPrice = x.ProductTotalPrice
        }).ToList();
    }
}