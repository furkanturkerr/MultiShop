namespace MultiShop.Dtos.BasketDtos;

public class BasketTotalDto
{
    public string UserId { get; set; } = string.Empty;
    public string DiscountCode { get; set; } = string.Empty;
    public int? DiscountRate { get; set; }
    public List<BasketItemDto> BasketItems { get; set; } = [];
    public decimal TotalPrice
    {
        get => BasketItems.Sum(x => x.ProductPrice * x.Quantity);
    }

    public decimal DiscountAmount => TotalPrice * (DiscountRate ?? 0) / 100;
    public decimal TotalPriceAfterDiscount => TotalPrice - DiscountAmount;
}
