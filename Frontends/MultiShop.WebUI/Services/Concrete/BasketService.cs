using MultiShop.Dtos.BasketDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class BasketService : IBasketService
{
    private readonly HttpClient _client;
    private readonly IProductService _productService;

    public BasketService(IHttpClientFactory httpClientFactory, IProductService productService)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
        _productService = productService;
    }
    
    public async Task<BasketTotalDto> GetBasketAsync()
    {
        var response = await _client.GetAsync("basket/Baskets");
        if (!response.IsSuccessStatusCode)
            return new BasketTotalDto();

        var basket = await response.Content.ReadFromJsonAsync<BasketTotalDto>() ?? new BasketTotalDto();
        var basketUpdated = false;

        foreach (var item in basket.BasketItems.Where(x => string.IsNullOrWhiteSpace(x.ProductImageUrl)))
        {
            var product = await _productService.GetByIdProductAsync(item.ProductId);
            if (product is null)
                continue;

            item.ProductImageUrl = product.ProductImageUrl;
            basketUpdated = true;
        }

        if (basketUpdated)
            await SaveBasketAsync(basket);

        return basket;
    }

    public async Task SaveBasketAsync(BasketTotalDto basket)
    {
        var response = await _client.PostAsJsonAsync("basket/Baskets", basket);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteBasketAsync(string userId)
    {
        await _client.DeleteAsync("basket/Baskets");
    }

    public async Task AddBasketItemAsync(BasketItemDto basketItem)
    {
        var values = await GetBasketAsync();
        if (!values.BasketItems.Any(x => x.ProductId == basketItem.ProductId))
        {
            values.BasketItems.Add(basketItem);
        }
        else
        {
            var existingItem = values.BasketItems.First(x => x.ProductId == basketItem.ProductId);
            existingItem.ProductName = basketItem.ProductName;
            existingItem.ProductImageUrl = basketItem.ProductImageUrl;
            existingItem.ProductPrice = basketItem.ProductPrice;
            existingItem.Quantity += basketItem.Quantity;
        }
        await SaveBasketAsync(values);
    }

    public async Task RemoveBasketItemAsync(string productId)
    {
        var values = await GetBasketAsync();
        var basketItem = values.BasketItems.FirstOrDefault(x => x.ProductId == productId);
        if (basketItem is not null && values.BasketItems.Remove(basketItem))
            await SaveBasketAsync(values);
    }

    public async Task<BasketTotalDto?> UpdateBasketItemQuantityAsync(string productId, int quantity)
    {
        if (quantity < 1)
            return null;

        var basket = await GetBasketAsync();
        var basketItem = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
        if (basketItem is null)
            return null;

        basketItem.Quantity = quantity;
        await SaveBasketAsync(basket);
        return basket;
    }
}
