using MultiShop.Dtos.CatalogDtos.ProductDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IProductService
{
    Task<List<ResultProductDto>> GetAllProductAsync();
    Task<UpdateProductDto?> GetByIdProductAsync(string id);
    Task<bool> CreateProductAsync(CreateProductDto dto);
    Task<bool> UpdateProductAsync(UpdateProductDto dto);
    Task DeleteProductAsync(string id);
}
