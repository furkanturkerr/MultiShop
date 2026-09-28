namespace MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;

public class GetOrderingByIdQueryResult
{
    public int OrderingId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int AddressId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
}
