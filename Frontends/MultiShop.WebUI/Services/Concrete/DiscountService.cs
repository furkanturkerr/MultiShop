using MultiShop.Dtos.DiscountDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete;

public class DiscountService : IDiscountService
{
    private readonly HttpClient _client;

    public DiscountService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("GatewayApi");
    }

    public async Task<List<ResultCouponDto>> GetAllCouponsAsync()
    {
        var response = await _client.GetAsync("discount/Discount");
        if (!response.IsSuccessStatusCode)
            return [];

        return await response.Content.ReadFromJsonAsync<List<ResultCouponDto>>() ?? [];
    }

    public async Task<UpdateCouponDto?> GetCouponByIdAsync(int id)
    {
        var coupon = (await GetAllCouponsAsync()).FirstOrDefault(x => x.CouponId == id);
        if (coupon is null)
            return null;

        return new UpdateCouponDto
        {
            CouponId = coupon.CouponId,
            Code = coupon.Code,
            Rate = coupon.Rate,
            IsActive = coupon.IsActive,
            ValidDate = coupon.ValidDate
        };
    }

    public async Task<bool> CreateCouponAsync(CreateCouponDto dto)
    {
        var response = await _client.PostAsJsonAsync("discount/Discount", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCouponAsync(UpdateCouponDto dto)
    {
        var response = await _client.PutAsJsonAsync("discount/Discount", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCouponAsync(int id)
    {
        var response = await _client.DeleteAsync($"discount/Discount/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<GetDiscountCodeDetailByCode?> GetDiscountCodeAsync(string code)
    {
        var address = $"discount/Discount/GetCouponByCode?code={Uri.EscapeDataString(code)}";
        var response = await _client.GetAsync(address);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<GetDiscountCodeDetailByCode>();
    }
}
