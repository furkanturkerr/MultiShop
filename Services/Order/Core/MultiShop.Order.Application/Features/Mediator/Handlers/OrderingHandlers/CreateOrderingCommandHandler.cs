using MediatR;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers;

public class CreateOrderingCommandHandler : IRequestHandler<CreateOrderingCommand, int>
{
    private readonly IRepository<Ordering> _orderingRepository;

    public CreateOrderingCommandHandler(IRepository<Ordering> orderingRepository)
    {
        _orderingRepository = orderingRepository;
    }

    public async Task<int> Handle(CreateOrderingCommand request, CancellationToken cancellationToken)
    {
        var ordering = new Ordering
        {
            AddressId = request.AddressId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = "Sipariş alındı",
            PaymentMethod = request.PaymentMethod,
            TotalPrice = request.TotalPrice,
            UserId = request.UserId,
            OrderDetails = request.OrderDetails
        };
        
        await _orderingRepository.CreateAsync(ordering);
        return ordering.OrderingId;
    }
}
