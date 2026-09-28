using MultiShop.Dtos.DiscountDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IDiscountService
{
    Task<List<ResultCouponDto>> GetAllCouponsAsync();
    Task<UpdateCouponDto?> GetCouponByIdAsync(int id);
    Task<bool> CreateCouponAsync(CreateCouponDto dto);
    Task<bool> UpdateCouponAsync(UpdateCouponDto dto);
    Task<bool> DeleteCouponAsync(int id);
    Task<GetDiscountCodeDetailByCode?> GetDiscountCodeAsync(string code);
}
