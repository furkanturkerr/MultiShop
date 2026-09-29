using System.Text.Json;
using MultiShop.MessageBus;
using MultiShop.MessageBus.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MultiShop.OrderCreatedConsumer;

public sealed class OrderCreatedConsumer : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public OrderCreatedConsumer(
        RabbitMqSettings settings,
        ILogger<OrderCreatedConsumer> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await StartConsumerAsync(stoppingToken);
                await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "RabbitMQ bağlantısı kurulamadı. 5 saniye sonra yeniden denenecek.");

                await DisposeConnectionAsync();
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task StartConsumerAsync(CancellationToken cancellationToken)
    {
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
            "multishop-order-created-consumer",
            cancellationToken);
        _channel = await _connection.CreateChannelAsync(
            cancellationToken: cancellationToken);

        await RabbitMqPublisher.DeclareTopologyAsync(
            _channel,
            _settings,
            cancellationToken);

        await _channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += HandleMessageAsync;

        await _channel.BasicConsumeAsync(
            queue: _settings.OrderCreatedQueue,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);

        _logger.LogInformation(
            "RabbitMQ dinleniyor. Kuyruk: {QueueName}",
            _settings.OrderCreatedQueue);
    }

    private async Task HandleMessageAsync(
        object sender,
        BasicDeliverEventArgs eventArgs)
    {
        try
        {
            var message = JsonSerializer.Deserialize<OrderCreatedEvent>(
                eventArgs.Body.Span);

            if (message is null)
                throw new JsonException("OrderCreatedEvent mesajı okunamadı.");

            _logger.LogInformation(
                "Sipariş mesajı alındı. OrderId: {OrderId}, UserId: {UserId}, Tutar: {TotalPrice}",
                message.OrderId,
                message.UserId,
                message.TotalPrice);

            await _channel!.BasicAckAsync(
                deliveryTag: eventArgs.DeliveryTag,
                multiple: false);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Sipariş mesajı işlenemedi.");

            await _channel!.BasicNackAsync(
                deliveryTag: eventArgs.DeliveryTag,
                multiple: false,
                requeue: false);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await DisposeConnectionAsync();
        await base.StopAsync(cancellationToken);
    }

    private async Task DisposeConnectionAsync()
    {
        if (_channel is not null)
        {
            await _channel.DisposeAsync();
            _channel = null;
        }

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }
    }
}
