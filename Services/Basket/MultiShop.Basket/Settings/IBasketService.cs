using MultiShop.Basket.Dtos;

namespace MultiShop.Basket.Settings;

public interface IBasketService
{
    Task<BasketTotalDto> GetBasketAsync(string userId);
    Task SaveBasketAsync(BasketTotalDto basket);
    Task DeleteBasketAsync(string userId);
}