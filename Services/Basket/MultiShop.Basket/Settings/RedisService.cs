using StackExchange.Redis;

namespace MultiShop.Basket.Settings;

public class RedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public IDatabase GetDb()
    {
        return _connectionMultiplexer.GetDatabase();
    }
}