using System.ComponentModel.DataAnnotations;

namespace MultiShop.Dtos.BasketDtos;

public class AddBasketItemDto
{
    [Required]
    [RegularExpression("^[a-fA-F0-9]{24}$")]
    public string ProductId { get; set; } = string.Empty;
    [Range(1, 99)]
    public int Quantity { get; set; } = 1;
    public Dictionary<string, string> SelectedOptions { get; set; } = new();
}
