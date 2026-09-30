using Moq;
using MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Tests;

public class GetOrderingByIdHandlerTests
{
    
    //Handler’ın:
    // 1. Doğru sipariş ID’sini repository’ye göndermesini,
    // 2. Gelen Ordering nesnesini doğru sonuç modeline çevirmesini
    
    [Test]
    public async Task Handle_SiparisBulundugunda_SiparisBilgileriniDoner()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Ordering>>();

        var order = new Ordering
        {
            OrderingId = 12,
            UserId = "user-123",
            AddressId = 5,
            TotalPrice = 1500,
            PaymentMethod = "Kredi Kartı",
            OrderStatus = "Sipariş alındı"
        };

        repositoryMock
            .Setup(x => x.GetByIdAsync(12))
            .ReturnsAsync(order);

        var handler = new GetOrderingByIdHandler(repositoryMock.Object);
        var query = new GetOrderingByIdQuery(12);

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        //Bir nesnenin birden fazla alanını beraber kontrol eder. Birkaç alan hatalıysa NUnit hepsini aynı test sonucunda gösterebilir.
        Assert.Multiple(() =>
        {
            Assert.That(result.OrderingId, Is.EqualTo(12));
            Assert.That(result.UserId, Is.EqualTo("user-123"));
            Assert.That(result.TotalPrice, Is.EqualTo(1500));
            Assert.That(result.OrderStatus, Is.EqualTo("Sipariş alındı"));
        });

        repositoryMock.Verify(
            x => x.GetByIdAsync(12),
            Times.Once);
    }
}