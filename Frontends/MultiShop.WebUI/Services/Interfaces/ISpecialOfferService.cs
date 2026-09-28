using MultiShop.Dtos.CatalogDtos.SpecialOfferDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface ISpecialOfferService
{
    Task<List<ResultSpecialOfferDto>> GetAllSpecialOfferAsync();
    Task<List<ResultSpecialOfferDto>> GetActiveSpecialOfferAsync();
    Task<UpdateSpecialOfferDto?> GetByIdSpecialOfferAsync(string id);
    Task<bool> CreateSpecialOfferAsync(CreateSpecialOfferDto dto);
    Task<bool> UpdateSpecialOfferAsync(UpdateSpecialOfferDto dto);
    Task DeleteSpecialOfferAsync(string id);
}
