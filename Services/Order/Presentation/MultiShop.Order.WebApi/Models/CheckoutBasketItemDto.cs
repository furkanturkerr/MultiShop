namespace MultiShop.Order.WebApi.Models;

public class CheckoutBasketItemDto
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
    public Dictionary<string, string> SelectedOptions { get; set; } = new();
}
