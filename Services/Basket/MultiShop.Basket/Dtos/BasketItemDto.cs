namespace MultiShop.Basket.Dtos;

public class BasketItemDto
{
    public string BasketItemId { get; set; } = string.Empty;
    public Dictionary<string, string> SelectedOptions { get; set; } = new();
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductImageUrl { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
}
