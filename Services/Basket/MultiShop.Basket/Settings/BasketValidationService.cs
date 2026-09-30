using System.Net;
using MultiShop.Basket.Dtos;

namespace MultiShop.Basket.Settings;

public class BasketValidationService
{
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _contextAccessor;

    public BasketValidationService(HttpClient client, IHttpContextAccessor contextAccessor)
    {
        _client = client;
        _contextAccessor = contextAccessor;
    }

    public async Task ValidateAsync(BasketTotalDto basket)
    {
        if (basket.BasketItems is null || basket.BasketItems.Count > 100)
            throw new ArgumentException("Sepette en fazla 100 ürün satırı olabilir.");

        var cancellationToken = _contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
        var products = new Dictionary<string, BasketProductDto>();
        var rowIds = new HashSet<string>();

        foreach (var item in basket.BasketItems)
        {
            ValidateItem(item, rowIds);

            if (!products.TryGetValue(item.ProductId, out var product))
            {
                product = await GetProductAsync(item.ProductId, cancellationToken);
                products[item.ProductId] = product;
            }

            ValidateOptions(item, product.Options);
            item.ProductPrice = product.ProductPrice;
            item.ProductName = product.ProductName;
            item.ProductImageUrl = product.ProductImageUrl;
        }

        await ApplyDiscountAsync(basket, cancellationToken);
    }

    private static void ValidateItem(BasketItemDto item, HashSet<string> rowIds)
    {
        if (item.Quantity is < 1 or > 99 || string.IsNullOrWhiteSpace(item.ProductId) ||
            item.ProductId.Length != 24 || !item.ProductId.All(Uri.IsHexDigit) || item.SelectedOptions is null)
            throw new ArgumentException("Ürün veya adet geçersiz.");

        if (string.IsNullOrWhiteSpace(item.BasketItemId))
            item.BasketItemId = Guid.NewGuid().ToString();

        if (!rowIds.Add(item.BasketItemId))
            throw new ArgumentException("Sepet satırları farklı kimliklere sahip olmalı.");
    }

    private async Task<BasketProductDto> GetProductAsync(string productId, CancellationToken cancellationToken)
    {
        using var response = await _client.GetAsync(
            $"catalog/Products/{Uri.EscapeDataString(productId)}", cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.BadRequest)
            throw new ArgumentException("Ürün artık mevcut değil.");

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BasketProductDto>(cancellationToken: cancellationToken)
            ?? throw new ArgumentException("Ürün bilgisi alınamadı.");
    }

    private static void ValidateOptions(BasketItemDto item, List<BasketProductOptionDto> productOptions)
    {
        var options = productOptions.Where(x => x.Values.Count > 0).ToList();
        if (item.SelectedOptions.Count != options.Count)
            throw new ArgumentException("Ürünün geçerli seçeneklerini seçin.");

        foreach (var option in options)
        {
            if (!item.SelectedOptions.TryGetValue(option.Name, out var value) || !option.Values.Contains(value))
                throw new ArgumentException("Ürünün geçerli seçeneklerini seçin.");
        }
    }

    private async Task ApplyDiscountAsync(BasketTotalDto basket, CancellationToken cancellationToken)
    {
        basket.DiscountRate = null;
        if (string.IsNullOrWhiteSpace(basket.DiscountCode))
            return;

        if (basket.DiscountCode.Length > 100)
            throw new ArgumentException("Kupon kodu geçersiz.");

        using var request = new HttpRequestMessage(HttpMethod.Get,
            $"discount/Discount/GetCouponByCode?code={Uri.EscapeDataString(basket.DiscountCode)}");
        request.Headers.TryAddWithoutValidation("Authorization", _contextAccessor.HttpContext?.Request.Headers.Authorization.ToString());
        using var response = await _client.SendAsync(request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.NoContent)
        {
            basket.DiscountCode = string.Empty;
            return;
        }

        response.EnsureSuccessStatusCode();
        var coupon = await response.Content.ReadFromJsonAsync<BasketCouponDto>(cancellationToken: cancellationToken);
        if (coupon is null || !coupon.IsActive || coupon.Rate is < 1 or > 100 || coupon.ValidDate < DateTime.Today)
        {
            basket.DiscountCode = string.Empty;
            return;
        }

        basket.DiscountRate = coupon.Rate;
    }
}
