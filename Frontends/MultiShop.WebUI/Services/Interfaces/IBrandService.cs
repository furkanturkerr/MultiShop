using MultiShop.Dtos.CatalogDtos.BrandDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IBrandService
{
    Task<List<ResultBrandDto>> GetAllBrandAsync();
    Task<UpdateBrandDto?> GetByIdBrandAsync(string id);
    Task<bool> CreateBrandAsync(CreateBrandDto dto);
    Task<bool> UpdateBrandAsync(UpdateBrandDto dto);
    Task DeleteBrandAsync(string id);
}
