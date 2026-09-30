using MultiShop.Dtos.OrderDtos.OrderDetailDtos;

namespace MultiShop.Dtos.OrderDtos.OrderingDtos;

public class ResultOrderingDto
{
    public int OrderingId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int AddressId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
    public List<ResultOrderDetailDto> OrderDetails { get; set; } = [];
}
