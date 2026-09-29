using System.Text.Json;
using MultiShop.MessageBus.Events;
using RabbitMQ.Client;

namespace MultiShop.MessageBus;

public sealed class RabbitMqPublisher : IRabbitMqPublisher, IAsyncDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private readonly SemaphoreSlim _publishLock = new(1, 1);
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqPublisher(RabbitMqSettings settings)
    {
        _settings = settings;
    }

    public async Task PublishOrderCreatedAsync(
        OrderCreatedEvent message,
        CancellationToken cancellationToken = default)
    {
        await EnsureConnectedAsync(cancellationToken);

        var body = JsonSerializer.SerializeToUtf8Bytes(message);
        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = Guid.NewGuid().ToString("N"),
            Type = nameof(OrderCreatedEvent)
        };

        await _publishLock.WaitAsync(cancellationToken);
        try
        {
            await _channel!.BasicPublishAsync(
                exchange: _settings.ExchangeName,
                routingKey: _settings.OrderCreatedRoutingKey,
                mandatory: true,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);
        }
        finally
        {
            _publishLock.Release();
        }
    }

    private async Task EnsureConnectedAsync(CancellationToken cancellationToken)
    {
        if (_channel?.IsOpen == true)
            return;

        await _connectionLock.WaitAsync(cancellationToken);
        try
        {
            if (_channel?.IsOpen == true)
                return;

            if (_channel is not null)
                await _channel.DisposeAsync();
            if (_connection is not null)
                await _connection.DisposeAsync();

            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true
            };

            _connection = await factory.CreateConnectionAsync(
                "multishop-order-api",
                cancellationToken);
            _channel = await _connection.CreateChannelAsync(
                cancellationToken: cancellationToken);

            await DeclareTopologyAsync(_channel, _settings, cancellationToken);
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public static async Task DeclareTopologyAsync(
        IChannel channel,
        RabbitMqSettings settings,
        CancellationToken cancellationToken)
    {
        await channel.ExchangeDeclareAsync(
            exchange: settings.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: settings.OrderCreatedQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: settings.OrderCreatedQueue,
            exchange: settings.ExchangeName,
            routingKey: settings.OrderCreatedRoutingKey,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
        if (_connection is not null)
            await _connection.DisposeAsync();

        _connectionLock.Dispose();
        _publishLock.Dispose();
    }
}
