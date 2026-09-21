using MultiShop.Catalog.Dtos.OfferDiscountDtos;

namespace MultiShop.Catalog.Services.OfferDiscountServices;

public interface IOfferDiscountService
{
    Task<List<ResultOfferDiscountDto>> GetAllOfferDiscountAsync();

    Task CreateOfferDiscountAsync(CreateOfferDiscountDto dto);

    Task UpdateOfferDiscountAsync(UpdateOfferDiscountDto dto);

    Task<UpdateOfferDiscountDto> GetOfferDiscountByIdAsync(string id);

    Task DeleteOfferDiscountByIdAsync(string id);
    
    Task<List<ResultOfferDiscountDto>> GetOfferDiscountByStatusAsync();
}