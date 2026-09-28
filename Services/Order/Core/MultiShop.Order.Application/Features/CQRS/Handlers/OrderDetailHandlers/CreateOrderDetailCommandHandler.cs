using MultiShop.Order.Application.Features.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Handlers.OrderDetailHandlers;

public class CreateOrderDetailCommandHandler
{
    private readonly IRepository<OrderDetail> _orderDetailRepository;

    public CreateOrderDetailCommandHandler(IRepository<OrderDetail> orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task Handler(CreateOrderDetailCommand command)
    {
        await _orderDetailRepository.CreateAsync(new OrderDetail
        {
            ProductId = command.ProductId,
            ProductAmount = command.ProductAmount,
            ProductName = command.ProductName,
            ProductPrice = command.ProductPrice,
            ProductTotalPrice = command.ProductTotalPrice,
            OrderingId = command.OrderingId
        });
    }
}
