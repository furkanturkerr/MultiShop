using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services;

public interface IDiscountService
{
    Task<List<ResultCouponDto>> GetAllCouponsAsync();
    Task CreateCouponAsync(CreateCouponDto dto);
    Task UpdateCouponAsync(UpdateCouponDto dto);
    Task DeleteCouponAsync(int id);
    Task<GetByIdCouponDto> GetCouponAsync(int id);
    Task<ResultCouponDto> GetDiscountCodeDetailByCode(string code);
}