using MultiShop.Catalog.Dtos.SpecialOfferDtos;

namespace MultiShop.Catalog.Services.SpecialOfferServices;

public interface ISpecialOfferService
{
    Task<List<ResultSpecialOfferDto>> GetAllSpecialOfferAsync();

    Task CreateSpecialOfferAsync(CreateSpecialOfferDto dto);

    Task UpdateSpecialOfferAsync(UpdateSpecialOfferDto dto);

    Task<UpdateSpecialOfferDto> GetSpecialOfferByIdAsync(string id);

    Task DeleteSpecialOfferByIdAsync(string id);
    
    Task<List<ResultSpecialOfferDto>> GetSpecialOfferByStatusAsync();
}