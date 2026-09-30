using MultiShop.Catalog.Dtos.CategoryDtos;

namespace MultiShop.Catalog.Dtos.ProductDtos;

public class ProductListResultDto
{
    public ProductFilterDto Filters { get; set; } = new();
    public List<ResultProductWithCategory> Products { get; set; } = [];
    public List<ResultCategoryDto> Categories { get; set; } = [];
    public Dictionary<string, int> CategoryCounts { get; set; } = [];
    public int CatalogCount { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
