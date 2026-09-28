using MultiShop.Dtos.CatalogDtos.OfferDiscountDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IOfferDiscountService
{
    Task<List<ResultOfferDiscountDto>> GetAllOfferDiscountAsync();
    Task<List<ResultOfferDiscountDto>> GetActiveOfferDiscountAsync();
    Task<UpdateOfferDiscountDto?> GetByIdOfferDiscountAsync(string id);
    Task<bool> CreateOfferDiscountAsync(CreateOfferDiscountDto dto);
    Task<bool> UpdateOfferDiscountAsync(UpdateOfferDiscountDto dto);
    Task DeleteOfferDiscountAsync(string id);
}
