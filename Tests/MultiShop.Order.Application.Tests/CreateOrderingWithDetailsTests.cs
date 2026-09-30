using Moq;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Tests;

public class CreateOrderingWithDetailsTests
{
    [Test]
    public async Task Handle_SiparisVeUrunleriBirlikteKaydeder()
    {
        var repository = new Mock<IRepository<Ordering>>();
        var handler = new CreateOrderingCommandHandler(repository.Object);
        var command = new CreateOrderingCommand
        {
            UserId = "user-123", AddressId = 1, TotalPrice = 180,
            OrderDetails = [new OrderDetail { ProductId = "product-1", ProductName = "Ürün", ProductPrice = 100, ProductAmount = 2, ProductTotalPrice = 200 }]
        };

        await handler.Handle(command, CancellationToken.None);

        repository.Verify(x => x.CreateAsync(It.Is<Ordering>(order =>
            order.UserId == "user-123" && order.TotalPrice == 180 &&
            order.OrderDetails.Count == 1 && order.OrderDetails[0].ProductId == "product-1")), Times.Once);
    }
}
