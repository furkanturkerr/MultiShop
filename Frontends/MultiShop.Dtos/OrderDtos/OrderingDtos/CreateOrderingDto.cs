namespace MultiShop.Dtos.OrderDtos.OrderingDtos;

public class CreateOrderingDto
{
    public string UserId { get; set; } = string.Empty;
    public int AddressId { get; set; }
    public decimal TotalPrice { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
}
