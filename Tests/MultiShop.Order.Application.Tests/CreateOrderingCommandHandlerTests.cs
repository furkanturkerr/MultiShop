using Moq;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Tests;

public class CreateOrderingCommandHandlerTests
{
    [Test]
    public async Task Handle_GecerliKomutGeldiginde_SiparisOlusturur()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Ordering>>();
        Ordering? savedOrder = null;

        repositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Ordering>()))
            .Callback<Ordering>(order =>
            {
                savedOrder = order;
                order.OrderingId = 42;
            })
            .Returns(Task.CompletedTask);

        var handler = new CreateOrderingCommandHandler(
            repositoryMock.Object);

        var command = new CreateOrderingCommand
        {
            UserId = "user-123",
            AddressId = 5,
            TotalPrice = 1500,
            PaymentMethod = "Kredi Kartı"
        };

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.That(savedOrder, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(savedOrder!.UserId, Is.EqualTo("user-123"));
            Assert.That(savedOrder.AddressId, Is.EqualTo(5));
            Assert.That(savedOrder.TotalPrice, Is.EqualTo(1500m));
            Assert.That(savedOrder.PaymentMethod, Is.EqualTo("Kredi Kartı"));
            Assert.That(savedOrder.OrderStatus, Is.EqualTo("Sipariş alındı"));
            Assert.That(result, Is.EqualTo(42));
        });

        repositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<Ordering>()),
            Times.Once);
        
        //It.IsAny<Ordering>() : CreateAsync metoduna herhangi bir Ordering nesnesi gelirse bu ayar çalışsın.
    }
}