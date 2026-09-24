using MultiShop.Catalog.Dtos.ProductDetailDtos;

namespace MultiShop.Catalog.Services.ProductDetailServices;

public interface IProductDetailService
{
    Task<List<ResultProductDetailDto>> GetAllCategoriesAsync();

    Task CreateProductDetailAsync(CreateProductDetailDto dto);

    Task UpdateProductDetailAsync(UpdateProductDetailDto dto);

    Task<GetByIdProductDetailDto> GetProductDetailByIdAsync(string id);

    Task DeleteProductDetailByIdAsync(string id);
    
    Task<GetByIdProductDetailDto> GetProductDetailByProductIdAsync(string productId);
}