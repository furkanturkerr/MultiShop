using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.OfferDiscountDtos;
using MultiShop.Catalog.Services.OfferDiscountServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferDiscountController : ControllerBase
    {
        private readonly IOfferDiscountService  _OfferDiscountService;

        public OfferDiscountController(IOfferDiscountService OfferDiscountService)
        {
            _OfferDiscountService = OfferDiscountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> OfferDiscountList()
        {
            var values = await _OfferDiscountService.GetAllOfferDiscountAsync();
            return Ok(values);
        }
        
        [HttpGet("status")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOfferDiscountActiveStatus()
        {
            var values = await _OfferDiscountService.GetOfferDiscountByStatusAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> OfferDiscountById(string id)
        {
            var value = await _OfferDiscountService.GetOfferDiscountByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOfferDiscount(CreateOfferDiscountDto OfferDiscountDto)
        {
            await _OfferDiscountService.CreateOfferDiscountAsync(OfferDiscountDto);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOfferDiscount(UpdateOfferDiscountDto OfferDiscountDto)
        {
            await _OfferDiscountService.UpdateOfferDiscountAsync(OfferDiscountDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOfferDiscount(string id)
        {
            await _OfferDiscountService.DeleteOfferDiscountByIdAsync(id);
            return Ok();
        }
    }
}
