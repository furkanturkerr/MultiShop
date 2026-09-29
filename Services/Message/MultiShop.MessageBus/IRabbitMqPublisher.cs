using MultiShop.MessageBus.Events;

namespace MultiShop.MessageBus;

public interface IRabbitMqPublisher
{
    Task PublishOrderCreatedAsync(
        OrderCreatedEvent message,
        CancellationToken cancellationToken = default);
}
