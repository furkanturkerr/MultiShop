using MultiShop.Dtos.CatalogDtos.FeaturedDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IFeaturedService
{
    Task<List<ResultFeaturedDto>> GetAllFeaturedAsync();
    Task<List<ResultFeaturedDto>> GetActiveFeaturedAsync();
    Task<UpdateFeaturedDto?> GetByIdFeaturedAsync(string id);
    Task<bool> CreateFeaturedAsync(CreateFeaturedDto dto);
    Task<bool> UpdateFeaturedAsync(UpdateFeaturedDto dto);
    Task DeleteFeaturedAsync(string id);
}
