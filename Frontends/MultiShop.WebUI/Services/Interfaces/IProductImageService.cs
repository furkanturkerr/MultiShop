using MultiShop.Dtos.CatalogDtos.ProductImageDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IProductImageService
{
    Task<UpdateProductImageDto?> GetProductImageByProductIdAsync(string productId);
    Task<bool> UpdateProductImageAsync(UpdateProductImageDto dto);
}
