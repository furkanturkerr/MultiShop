namespace MultiShop.Dtos.CatalogDtos.OfferDiscountDtos;

public class CreateOfferDiscountDto
{
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string ButtonText { get; set; }
}