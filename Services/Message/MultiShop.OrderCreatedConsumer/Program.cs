using MultiShop.MessageBus;
using MultiShop.OrderCreatedConsumer;

var builder = Host.CreateApplicationBuilder(args);

var rabbitMqSettings = builder.Configuration
    .GetSection("RabbitMQ")
    .Get<RabbitMqSettings>()
    ?? throw new InvalidOperationException("RabbitMQ ayarları bulunamadı.");

builder.Services.AddSingleton(rabbitMqSettings);
builder.Services.AddHostedService<OrderCreatedConsumer>();

var host = builder.Build();
host.Run();
