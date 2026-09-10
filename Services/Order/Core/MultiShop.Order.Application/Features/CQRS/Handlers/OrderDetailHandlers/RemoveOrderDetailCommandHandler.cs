using MultiShop.Order.Application.Features.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Handlers.OrderDetailHandlers;

public class RemoveOrderDetailCommandHandler
{
    private readonly IRepository<OrderDetail> _orderDetailRepository;

    public RemoveOrderDetailCommandHandler(IRepository<OrderDetail> orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task Handle(RemoveOrderDetailCommand command)
    {
        var value = await _orderDetailRepository.GetByIdAsync(command.Id);
        await _orderDetailRepository.DeleteAsync(value);
    }
}