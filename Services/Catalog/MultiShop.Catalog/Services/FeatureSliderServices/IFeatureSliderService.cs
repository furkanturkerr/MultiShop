using MultiShop.Catalog.Dtos.FeatureSldierDtos;

namespace MultiShop.Catalog.Services.FeatureSliderServices;

public interface IFeatureSliderService
{
    Task<List<ResultFeatureSliderDto>> GetAllFeatureSliderAsync();

    Task CreateFeatureSliderAsync(CreateFeatureSliderDto dto);

    Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto dto);

    Task<UpdateFeatureSliderDto> GetFeatureSliderByIdAsync(string id);

    Task DeleteFeatureSliderByIdAsync(string id);
    
    Task<List<ResultFeatureSliderDto>> GetSliderByStatusAsync();
}