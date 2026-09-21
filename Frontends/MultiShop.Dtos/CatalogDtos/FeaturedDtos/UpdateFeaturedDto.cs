namespace MultiShop.Dtos.CatalogDtos.FeaturedDtos;

public class UpdateFeaturedDto
{
    public string FeaturedId { get; set; }
    public string Icon { get; set; }
    public string Title { get; set; }
    
    public bool IsActive { get; set; }
}