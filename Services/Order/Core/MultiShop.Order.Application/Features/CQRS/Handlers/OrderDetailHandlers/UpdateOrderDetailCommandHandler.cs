using MultiShop.Order.Application.Features.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Handlers.OrderDetailHandlers;

public class UpdateOrderDetailCommandHandler
{
    private readonly IRepository<OrderDetail> _orderDetailRepository;

    public UpdateOrderDetailCommandHandler(IRepository<OrderDetail> orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task Handler(UpdateOrderDetailCommand command)
    {
        var value = await _orderDetailRepository.GetByIdAsync(command.OrderDetailId);
        value.OrderingId = command.OrderingId;
        value.ProductId = command.ProductId;
        value.ProductAmount = command.ProductAmount;
        value.ProductName = command.ProductName;
        value.ProductTotalPrice = command.ProductTotalPrice;
        await _orderDetailRepository.UpdateAsync(value);
    }
}