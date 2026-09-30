namespace MultiShop.Dtos.CatalogDtos.CategoryDtos;

public class UpdateCategoryDto : CategoryOptionSettingsDto
{
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
    
    public string ImageUrl { get; set; }
}