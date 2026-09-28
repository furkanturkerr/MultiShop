using MultiShop.Dtos.CatalogDtos.ProductDetailDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IProductDetailService
{
    Task<UpdateProductDetailDto?> GetProductDetailByProductIdAsync(string productId);
    Task<ResultProductDetailDto?> GetResultProductDetailByProductIdAsync(string productId);
    Task<bool> UpdateProductDetailAsync(UpdateProductDetailDto dto);
}
