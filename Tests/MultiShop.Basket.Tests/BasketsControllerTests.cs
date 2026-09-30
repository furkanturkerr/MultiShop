using Microsoft.AspNetCore.Mvc;
using Moq;
using MultiShop.Basket.Controllers;
using MultiShop.Basket.Dtos;
using MultiShop.Basket.LoginServices;
using MultiShop.Basket.Settings;

namespace MultiShop.Basket.Tests;

public class BasketsControllerTests
{
    [Test]
    public async Task DeleteMyBasket_KullanicininKendiSepetiniSiler()
    {
        // Sahte servisleri oluştur
        var basketServiceMock = new Mock<IBasketService>();
        var loginServiceMock = new Mock<ILoginService>();

        // Sahte LoginService kullanıcı ID'sini versin
        loginServiceMock
            .Setup(x => x.GetUserId)
            .Returns("user-123");

        // Controller'a sahte servisleri ver
        var controller = new BasketsController(
            basketServiceMock.Object,
            loginServiceMock.Object);

        // Controller metodunu çalıştır
        var result = await controller.DeleteMyBasket();

        // Doğru kullanıcı ID'siyle silme servisi çağrıldı mı?
        basketServiceMock.Verify(
            x => x.DeleteBasketAsync("user-123"),
            Times.Once);

        // Controller 200 OK döndürdü mü?
        Assert.That(result, Is.TypeOf<OkResult>());
    }
    
    [Test]
    public async Task GetMyBasketDetail_KullanicininSepetiniOkIleDoner()
    {
        // Arrange
        var basketServiceMock = new Mock<IBasketService>();
        var loginServiceMock = new Mock<ILoginService>();

        loginServiceMock
            .Setup(x => x.GetUserId)
            .Returns("user-123");

        var expectedBasket = new BasketTotalDto
        {
            UserId = "user-123",
            BasketItems =
            [
                new BasketItemDto
                {
                    ProductName = "Kulaklık",
                    ProductPrice = 1000,
                    Quantity = 2
                }
            ]
        };

        basketServiceMock
            .Setup(x => x.GetBasketAsync("user-123"))
            .ReturnsAsync(expectedBasket);

        var controller = new BasketsController(
            basketServiceMock.Object,
            loginServiceMock.Object);

        // Act
        var result = await controller.GetMyBasketDetail();

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());

        var okResult = (OkObjectResult)result;

        Assert.That(okResult.Value, Is.SameAs(expectedBasket));

        basketServiceMock.Verify(
            x => x.GetBasketAsync("user-123"),
            Times.Once);
    }
}