using MultiShop.Dtos.CatalogDtos.CategoryDtos;
using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.Models.Product;

public class ProductListViewModel
{
    public ProductFilterDto Filters { get; set; } = new();
    public List<ResultProductDto> Products { get; set; } = [];
    public List<ResultCategoryDto> Categories { get; set; } = [];
    public Dictionary<string, int> CategoryCounts { get; set; } = [];
    public int CatalogCount { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasFilters => !string.IsNullOrWhiteSpace(Filters.Q) || !string.IsNullOrWhiteSpace(Filters.CategoryId)
        || Filters.MinPrice.HasValue || Filters.MaxPrice.HasValue;
    public int FirstItem => TotalCount == 0 ? 0 : (Filters.Page - 1) * Filters.PageSize + 1;
    public int LastItem => Math.Min(Filters.Page * Filters.PageSize, TotalCount);
}
