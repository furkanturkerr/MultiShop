using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Discount.Dtos;
using MultiShop.Discount.Services;

namespace MultiShop.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var values = await _discountService.GetAllCouponsAsync();
            if (!User.IsInRole("Admin") && !User.IsInRole("Manager"))
                values = values.Where(x => x.IsActive && x.ValidDate >= DateTime.Today && x.Rate > 0 && x.Rate <= 100).ToList();
            return Ok(values);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await _discountService.GetCouponAsync(id);
            return Ok(value);
        }

        [HttpGet("GetCouponByCode")]
        public async Task<IActionResult> GetDiscountCodeDetailByCode(string code)
        {
            var value = await _discountService.GetDiscountCodeDetailByCode(code);
            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CreateCouponDto dto)
        {
            await _discountService.CreateCouponAsync(dto);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(UpdateCouponDto dto)
        {
            await _discountService.UpdateCouponAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _discountService.DeleteCouponAsync(id);
            return Ok();
        }
    }
}
