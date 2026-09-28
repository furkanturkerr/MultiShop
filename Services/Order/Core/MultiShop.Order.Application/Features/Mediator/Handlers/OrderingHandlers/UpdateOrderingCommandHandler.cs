using MediatR;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers;

public class UpdateOrderingCommandHandler : IRequestHandler<UpdateOrderingCommand>
{
    private readonly IRepository<Ordering> _orderingRepository;

    public UpdateOrderingCommandHandler(IRepository<Ordering> orderingRepository)
    {
        _orderingRepository = orderingRepository;
    }

    public async Task Handle(UpdateOrderingCommand request, CancellationToken cancellationToken)
    {
        var values = await _orderingRepository.GetByIdAsync(request.OrderingId);
        values.AddressId = request.AddressId;
        values.OrderStatus = request.OrderStatus;
        values.PaymentMethod = request.PaymentMethod;
        values.TotalPrice = request.TotalPrice;
        values.UserId = request.UserId;
        values.OrderDate = request.OrderDate;
        values.OrderingId = request.OrderingId;
        await _orderingRepository.UpdateAsync(values);
    }
}
