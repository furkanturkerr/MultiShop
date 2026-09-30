using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using MultiShop.Basket.Dtos;
using MultiShop.Basket.Settings;

namespace MultiShop.Basket.Tests.Services;

public class BasketValidationServiceTests
{
    [Test]
    public async Task ValidateAsync_IstemcininFiyatiniVeIndiriminiKabulEtmez()
    {
        var service = CreateService();
        var basket = CreateBasket();
        basket.DiscountCode = "TEST";
        basket.DiscountRate = 99;
        basket.BasketItems[0].ProductPrice = 1;
        basket.BasketItems[0].ProductName = "Sahte ürün";

        await service.ValidateAsync(basket);

        Assert.That(basket.BasketItems[0].ProductPrice, Is.EqualTo(350m));
        Assert.That(basket.BasketItems[0].ProductName, Is.EqualTo("Gerçek ürün"));
        Assert.That(basket.DiscountRate, Is.EqualTo(10));
    }

    [Test]
    public void ValidateAsync_UrundeOlmayanSecenegiReddeder()
    {
        var service = CreateService();
        var basket = CreateBasket();
        basket.BasketItems[0].SelectedOptions["Renk"] = "Mavi";

        Assert.ThrowsAsync<ArgumentException>(() => service.ValidateAsync(basket));
    }

    [Test]
    public async Task ValidateAsync_KuponsuzIndirimOraniniSiler()
    {
        var basket = CreateBasket();
        basket.DiscountRate = 100;

        await CreateService().ValidateAsync(basket);

        Assert.That(basket.DiscountRate, Is.Null);
    }

    [Test]
    public void ValidateAsync_NegatifAdediReddeder()
    {
        var basket = CreateBasket();
        basket.BasketItems[0].Quantity = -1;
        Assert.ThrowsAsync<ArgumentException>(() => CreateService().ValidateAsync(basket));
    }

    private static BasketTotalDto CreateBasket() => new()
    {
        BasketItems = [new BasketItemDto
        {
            ProductId = "507f1f77bcf86cd799439011", Quantity = 2,
            SelectedOptions = new() { ["Renk"] = "Siyah" }
        }]
    };

    private static BasketValidationService CreateService()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "Bearer test-token";
        return new BasketValidationService(
            new HttpClient(new ProductAndCouponHandler()) { BaseAddress = new Uri("http://test/services/") },
            new HttpContextAccessor { HttpContext = context });
    }

    // Gerçek katalog, kupon veya veritabanına bağlanmadan API cevaplarını taklit eder.
    private class ProductAndCouponHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri!.AbsolutePath.Contains("catalog/"))
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(new BasketProductDto
                    {
                        ProductName = "Gerçek ürün", ProductPrice = 350,
                        Options = [new BasketProductOptionDto { Name = "Renk", Values = ["Siyah"] }]
                    })
                });
            Assert.That(request.Headers.Authorization?.Parameter, Is.EqualTo("test-token"));
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new BasketCouponDto { IsActive = true, Rate = 10, ValidDate = DateTime.Today.AddDays(1) })
            });
        }
    }
}
