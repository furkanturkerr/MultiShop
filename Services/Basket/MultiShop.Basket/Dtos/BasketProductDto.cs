namespace MultiShop.Basket.Dtos;

public class BasketProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public string ProductImageUrl { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public List<BasketProductOptionDto> Options { get; set; } = [];
}
