using MultiShop.Dtos.BasketDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IBasketService
{
    Task<BasketTotalDto> GetBasketAsync();
    Task SaveBasketAsync(BasketTotalDto basket);
    Task DeleteBasketAsync();
    Task<bool> AddProductAsync(AddBasketItemDto selection);
    Task AddBasketItemAsync(BasketItemDto basketItem);
    Task RemoveBasketItemAsync(string basketItemId);
    Task<BasketTotalDto?> UpdateBasketItemQuantityAsync(string basketItemId, int quantity);
}
