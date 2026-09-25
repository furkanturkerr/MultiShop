using MultiShop.Dtos.CatalogDtos.FeatureSliderDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IFeatureSliderService
{
    Task<List<ResultFeatureSliderDto>> GetAllFeatureSliderAsync();
    Task<UpdateFeatureSliderDto?> GetByIdFeatureSliderAsync(string id);
    Task<bool> CreateFeatureSliderAsync(CreateFeatureSliderDto dto);
    Task<bool> UpdateFeatureSliderAsync(UpdateFeatureSliderDto dto);
    Task DeleteFeatureSliderAsync(string id);
}
