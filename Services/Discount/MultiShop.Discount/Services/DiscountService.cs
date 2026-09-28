using Dapper;
using MultiShop.Discount.Context;
using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services;

public class DiscountService : IDiscountService
{
    private readonly DapperContext _context;

    public DiscountService(DapperContext context)
    {
        _context = context;
    }

    public async Task<List<ResultCouponDto>> GetAllCouponsAsync()
    {
        string query = "select * from Coupons";
        using (var connection = _context.CreateConnection())
        {
            var coupons = await connection.QueryAsync<ResultCouponDto>(query);
            return coupons.ToList();
        }
    }

    public async Task CreateCouponAsync(CreateCouponDto dto)
    {
        string query =
            "Insert into Coupons (Code, Rate, IsActive, ValidDate) values (@Code, @Rate, @IsActive, @ValidDate)";
        var parameters = new { dto.Code, dto.Rate, dto.IsActive, dto.ValidDate };
        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task UpdateCouponAsync(UpdateCouponDto dto)
    {
        string query = "Update Coupons Set Code = @code, Rate = @rate, IsActive = @isActive, ValidDate = @validDate Where CouponId = @couponId";
        var parameters = new { dto.CouponId, dto.Code, dto.Rate, dto.IsActive, dto.ValidDate };
        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task DeleteCouponAsync(int id)
    {
        string query = "Delete from Coupons where CouponId = @couponId";
        var parameters = new { couponId = id };
        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task<GetByIdCouponDto> GetCouponAsync(int id)
    {
        string query = "Select * From Coupons Where CouponId = @couponId";
        var parameters = new { couponId = id };
        using (var connection = _context.CreateConnection())
        {
            var value = await connection.QueryFirstOrDefaultAsync<GetByIdCouponDto>(query, parameters);
            return value;
        }
    }

    public async Task<ResultCouponDto> GetDiscountCodeDetailByCode(string code)
    {
        string query = "Select * From Coupons Where Code = @code";
        var parameters = new { code = code };
        using (var connection = _context.CreateConnection())
        {
            var value = await connection.QueryFirstOrDefaultAsync<ResultCouponDto>(query, parameters);
            return value;
        }
    }
}