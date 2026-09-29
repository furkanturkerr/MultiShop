namespace MultiShop.MessageBus.Events;

public sealed record OrderCreatedEvent(
    int OrderId,
    string UserId,
    decimal TotalPrice,
    string PaymentMethod,
    DateTime CreatedAtUtc);
