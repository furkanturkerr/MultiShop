using MultiShop.Dtos.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Models.Product;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IProductService
{
    Task<List<ResultProductDto>> GetAllProductAsync();
    Task<ProductListViewModel> GetProductListAsync(ProductFilterDto filters);
    Task<List<ResultProductDto>> GetProductByCategoryIdAsync(string categoryId);
    Task<UpdateProductDto?> GetByIdProductAsync(string id);
    Task<bool> CreateProductAsync(CreateProductDto dto);
    Task<bool> UpdateProductAsync(UpdateProductDto dto);
    Task DeleteProductAsync(string id);
}
