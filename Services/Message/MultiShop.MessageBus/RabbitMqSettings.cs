namespace MultiShop.MessageBus;

public sealed class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "multishop";
    public string Password { get; set; } = "multishop_dev";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "multishop.events";
    public string OrderCreatedQueue { get; set; } = "multishop.order.created";
    public string OrderCreatedRoutingKey { get; set; } = "order.created";
}
