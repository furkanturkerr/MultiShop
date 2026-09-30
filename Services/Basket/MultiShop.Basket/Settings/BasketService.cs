using System.Text.Json;
using MultiShop.Basket.Dtos;

namespace MultiShop.Basket.Settings;

public class BasketService : IBasketService
{
    private readonly RedisService _redisService;
    private readonly BasketValidationService _validationService;

    public BasketService(RedisService redisService, BasketValidationService validationService)
    {
        _redisService = redisService;
        _validationService = validationService;
    }

    public async Task<BasketTotalDto> GetBasketAsync(string userId)
    {
        var existBasket = await _redisService.GetDb().StringGetAsync(userId);

        if (existBasket.IsNullOrEmpty)
        {
            return new BasketTotalDto { UserId = userId };
        }

        var basket = JsonSerializer.Deserialize<BasketTotalDto>(existBasket!)
               ?? new BasketTotalDto { UserId = userId };
        basket.UserId = userId;
        await _validationService.ValidateAsync(basket);
        return basket;
    }

    public async Task SaveBasketAsync(BasketTotalDto basket)
    {
        await _validationService.ValidateAsync(basket);
        await _redisService.GetDb().StringSetAsync(basket.UserId, JsonSerializer.Serialize(basket));
    }

    public async Task DeleteBasketAsync(string userId)
    {
        await _redisService.GetDb().KeyDeleteAsync(userId);
    }
}
