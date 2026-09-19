using MultiShop.Catalog.Dtos.ProductDtos;

namespace MultiShop.Catalog.Services.ProductServices;

public interface IProductService
{
    Task<List<ResultProductDto>> GetAllCategoriesAsync();

    Task CreateProductAsync(CreateProductDto dto);

    Task UpdateProductAsync(UpdateProductDto dto);

    Task<GetByIdProductDto> GetProductByIdAsync(string id);

    Task DeleteProductByIdAsync(string id);
    
    Task<List<ResultProductWithCategory>> GetAllProductsWithCategoryAsync();
}