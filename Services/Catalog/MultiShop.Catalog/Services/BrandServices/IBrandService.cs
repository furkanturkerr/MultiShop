using MultiShop.Catalog.Dtos.BrandDtos;

namespace MultiShop.Catalog.Services.BrandServices;

public interface IBrandService
{
    Task<List<ResultBrandDto>> GetAllBrandAsync();

    Task CreateBrandAsync(CreateBrandDto dto);

    Task UpdateBrandAsync(UpdateBrandDto dto);

    Task<UpdateBrandDto> GetBrandByIdAsync(string id);

    Task DeleteBrandByIdAsync(string id);
    
    Task<List<ResultBrandDto>> GetBrandByStatusAsync();
}