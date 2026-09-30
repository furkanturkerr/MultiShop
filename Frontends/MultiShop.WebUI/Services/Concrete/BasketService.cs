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
        using var response = await _client.GetAsync("basket/Baskets");
        if (!response.IsSuccessStatusCode)
            return new BasketTotalDto();

        var basket = await response.Content.ReadFromJsonAsync<BasketTotalDto>() ?? new BasketTotalDto();
        return basket;
    }

    public async Task SaveBasketAsync(BasketTotalDto basket)
    {
        using var response = await _client.PostAsJsonAsync("basket/Baskets", basket);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteBasketAsync()
    {
        using var response = await _client.DeleteAsync("basket/Baskets");
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> AddProductAsync(AddBasketItemDto selection)
    {
        if (selection.Quantity is < 1 or > 99 || selection.SelectedOptions is null)
            return false;

        var product = await _productService.GetByIdProductAsync(selection.ProductId);
        if (product is null || selection.SelectedOptions.Count != product.Options.Count)
            return false;

        var selectedOptions = new Dictionary<string, string>();
        foreach (var option in product.Options)
        {
            if (!selection.SelectedOptions.TryGetValue(option.Name, out var selected) || !option.Values.Contains(selected))
                return false;
            selectedOptions.Add(option.Name, selected);
        }

        await AddBasketItemAsync(new BasketItemDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            ProductImageUrl = product.ProductImageUrl,
            ProductPrice = product.ProductPrice,
            Quantity = selection.Quantity,
            SelectedOptions = selectedOptions
        });
        return true;
    }

    public async Task AddBasketItemAsync(BasketItemDto basketItem)
    {
        var values = await GetBasketAsync();
        var existingItem = values.BasketItems.FirstOrDefault(x =>
            x.ProductId == basketItem.ProductId &&
            x.SelectedOptions.Count == basketItem.SelectedOptions.Count &&
            x.SelectedOptions.All(option => basketItem.SelectedOptions.TryGetValue(option.Key, out var value) && value == option.Value));
        if (existingItem is null)
        {
            basketItem.BasketItemId = Guid.NewGuid().ToString("N");
            values.BasketItems.Add(basketItem);
        }
        else
        {
            existingItem.ProductName = basketItem.ProductName;
            existingItem.ProductImageUrl = basketItem.ProductImageUrl;
            existingItem.ProductPrice = basketItem.ProductPrice;
            existingItem.Quantity = Math.Min(99, existingItem.Quantity + basketItem.Quantity);
        }
        await SaveBasketAsync(values);
    }

    public async Task RemoveBasketItemAsync(string basketItemId)
    {
        var values = await GetBasketAsync();
        var basketItem = values.BasketItems.FirstOrDefault(x => x.BasketItemId == basketItemId);
        if (basketItem is not null && values.BasketItems.Remove(basketItem))
            await SaveBasketAsync(values);
    }

    public async Task<BasketTotalDto?> UpdateBasketItemQuantityAsync(string basketItemId, int quantity)
    {
        if (quantity is < 1 or > 99)
            return null;

        var basket = await GetBasketAsync();
        var basketItem = basket.BasketItems.FirstOrDefault(x => x.BasketItemId == basketItemId);
        if (basketItem is null)
            return null;

        basketItem.Quantity = quantity;
        await SaveBasketAsync(basket);
        return basket;
    }
}
