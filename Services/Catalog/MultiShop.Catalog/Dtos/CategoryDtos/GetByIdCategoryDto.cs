namespace MultiShop.Catalog.Dtos.CategoryDtos;

public class GetByIdCategoryDto : CategoryOptionSettingsDto
{
    public string CategoryId { get; set; }

    public string CategoryName { get; set; }
    
    public string ImageUrl { get; set; }
}