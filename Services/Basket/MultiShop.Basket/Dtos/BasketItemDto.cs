namespace MultiShop.Basket.Dtos;

public class BasketItemDto
{
    public string ProductId { get; set; } 
    public string ProductName { get; set; } 
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
}