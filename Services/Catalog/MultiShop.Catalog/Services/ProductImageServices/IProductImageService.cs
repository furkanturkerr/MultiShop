using MultiShop.Catalog.Dtos.ProductImageDtos;

namespace MultiShop.Catalog.Services.ProductImageServices;

public interface IProductImageService
{
    Task<List<ResultProductImageDto>> GetAllCategoriesAsync();

    Task CreateProductImageAsync(CreateProductImageDto dto);

    Task UpdateProductImageAsync(UpdateProductImageDto dto);

    Task<GetByIdProductImageDto> GetProductImageByIdAsync(string id);

    Task DeleteProductImageByIdAsync(string id);
}