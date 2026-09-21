using MultiShop.Catalog.Dtos.FeaturedDtos;

namespace MultiShop.Catalog.Services.FeaturedServices;

public interface IFeaturedService
{
    Task<List<ResultFeaturedDto>> GetAllFeaturedAsync();

    Task CreateFeaturedAsync(CreateFeaturedDto dto);

    Task UpdateFeaturedAsync(UpdateFeaturedDto dto);

    Task<UpdateFeaturedDto> GetFeaturedByIdAsync(string id);

    Task DeleteFeaturedByIdAsync(string id);
    
    Task<List<ResultFeaturedDto>> GetFeaturedByStatusAsync();
}