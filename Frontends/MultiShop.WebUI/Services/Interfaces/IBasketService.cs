using MultiShop.Dtos.BasketDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IBasketService
{
    Task<BasketTotalDto> GetBasketAsync();
    Task SaveBasketAsync(BasketTotalDto basket);
    Task DeleteBasketAsync(string userId);
    Task AddBasketItemAsync(BasketItemDto basketItem);
    Task RemoveBasketItemAsync(string productId);
    Task<BasketTotalDto?> UpdateBasketItemQuantityAsync(string productId, int quantity);
}
