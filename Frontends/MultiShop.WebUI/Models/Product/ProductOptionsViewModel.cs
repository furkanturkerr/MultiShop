using MultiShop.Dtos.CatalogDtos.CategoryDtos;
using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.Models.Product;

public class ProductOptionsViewModel
{
    public string CategoryId { get; set; } = string.Empty;
    public List<ResultCategoryDto> Categories { get; set; } = new();
    public List<ProductOptionDto> Options { get; set; } = new();
}
