using Microsoft.AspNetCore.Mvc;
using MultiShop.Discount.Dtos;
using MultiShop.Discount.Services;

namespace MultiShop.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _discountService.GetAllCouponsAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await _discountService.GetCouponAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCouponDto dto)
        {
            await _discountService.CreateCouponAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCouponDto dto)
        {
            await _discountService.UpdateCouponAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _discountService.DeleteCouponAsync(id);
            return Ok();
        }
    }
}
