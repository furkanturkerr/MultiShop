namespace MultiShop.Dtos.CatalogDtos.CategoryDtos;

public class CreateCategoryDto : CategoryOptionSettingsDto
{
    public string CategoryName { get; set; }
    
    public string ImageUrl { get; set; }
}