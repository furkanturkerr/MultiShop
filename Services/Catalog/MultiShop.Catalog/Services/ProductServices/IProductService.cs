using MultiShop.Catalog.Dtos.ProductDtos;

namespace MultiShop.Catalog.Services.ProductServices;

public interface IProductService
{
    Task<ProductListResultDto> GetFilteredProductsAsync(ProductFilterDto filters, CancellationToken cancellationToken = default);

    Task<List<ResultProductDto>> GetAllCategoriesAsync();

    Task<bool> CreateProductAsync(CreateProductDto dto);

    Task<bool> UpdateProductAsync(UpdateProductDto dto);

    Task<GetByIdProductDto?> GetProductByIdAsync(string id);

    Task DeleteProductByIdAsync(string id);
    
    Task<List<ResultProductWithCategory>> GetAllProductsWithCategoryAsync();
    
    Task<List<ResultProductWithCategory>> GetProductsByCategoryIdAsync(string categoryId);
}
