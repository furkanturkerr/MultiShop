using System.Text.Json;
using MultiShop.Basket.Dtos;

namespace MultiShop.Basket.Settings;

public class BasketService : IBasketService
{
    private readonly RedisService _redisService;

    public BasketService(RedisService redisService)
    {
        _redisService = redisService;
    }

    public async Task<BasketTotalDto> GetBasketAsync(string userId)
    {
        var existBasket = await _redisService.GetDb().StringGetAsync(userId);

        if (existBasket.IsNullOrEmpty)
        {
            return new BasketTotalDto { UserId = userId };
        }

        return JsonSerializer.Deserialize<BasketTotalDto>(existBasket!)
               ?? new BasketTotalDto { UserId = userId };
    }

    public async Task SaveBasketAsync(BasketTotalDto basket)
    {
        await _redisService.GetDb().StringSetAsync(basket.UserId, JsonSerializer.Serialize(basket));
    }

    public async Task DeleteBasketAsync(string userId)
    {
        await _redisService.GetDb().KeyDeleteAsync(userId);
    }
}
