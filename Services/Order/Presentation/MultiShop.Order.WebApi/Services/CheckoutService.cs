using System.Net;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Domain.Entities;
using MultiShop.Order.WebApi.Models;

namespace MultiShop.Order.WebApi.Services;

public class CheckoutService
{
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _contextAccessor;

    public CheckoutService(HttpClient client, IHttpContextAccessor contextAccessor)
    {
        _client = client;
        _contextAccessor = contextAccessor;
    }

    public async Task PrepareAsync(CreateOrderingCommand command)
    {
        var basket = await GetBasketAsync();
        command.OrderDetails = basket.BasketItems.Select(CreateOrderDetail).ToList();

        var subtotal = command.OrderDetails.Sum(x => x.ProductTotalPrice);
        var discountRate = basket.DiscountRate ?? 0;
        command.TotalPrice = decimal.Round(subtotal * (100 - discountRate) / 100m, 2);
    }

    private async Task<CheckoutBasketDto> GetBasketAsync()
    {
        var cancellationToken = _contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
        using var request = new HttpRequestMessage(HttpMethod.Get, "basket/Baskets");
        request.Headers.TryAddWithoutValidation("Authorization", _contextAccessor.HttpContext?.Request.Headers.Authorization.ToString());
        using var response = await _client.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            throw new ArgumentException("Sepette geçersiz ürün veya seçenek var.");

        response.EnsureSuccessStatusCode();
        var basket = await response.Content.ReadFromJsonAsync<CheckoutBasketDto>(cancellationToken: cancellationToken);
        if (basket is null || basket.BasketItems.Count == 0)
            throw new ArgumentException("Sepetiniz boş.");

        return basket;
    }

    private static OrderDetail CreateOrderDetail(CheckoutBasketItemDto item)
    {
        var productName = item.ProductName;
        if (item.SelectedOptions.Count > 0)
        {
            var options = string.Join(", ", item.SelectedOptions.Select(x => $"{x.Key}: {x.Value}"));
            productName += $" ({options})";
        }

        return new OrderDetail
        {
            ProductId = item.ProductId,
            ProductName = productName,
            ProductPrice = item.ProductPrice,
            ProductAmount = item.Quantity,
            ProductTotalPrice = item.ProductPrice * item.Quantity
        };
    }
}
