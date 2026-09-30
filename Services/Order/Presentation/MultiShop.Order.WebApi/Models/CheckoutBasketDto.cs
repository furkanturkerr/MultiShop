namespace MultiShop.Order.WebApi.Models;

public class CheckoutBasketDto
{
    public int? DiscountRate { get; set; }
    public List<CheckoutBasketItemDto> BasketItems { get; set; } = [];
}
