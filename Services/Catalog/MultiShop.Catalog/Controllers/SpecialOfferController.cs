using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.SpecialOfferDtos;
using MultiShop.Catalog.Services.SpecialOfferServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialOfferController : ControllerBase
    {
        private readonly ISpecialOfferService  _specialOfferService;

        public SpecialOfferController(ISpecialOfferService specialOfferService)
        {
            _specialOfferService = specialOfferService;
        }

        [HttpGet]
        public async Task<IActionResult> SpecialOfferList()
        {
            var values = await _specialOfferService.GetAllSpecialOfferAsync();
            return Ok(values);
        }
        
        [HttpGet("status")]
        public async Task<IActionResult> GetSpecialOfferActiveStatus()
        {
            var values = await _specialOfferService.GetSpecialOfferByStatusAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SpecialOfferById(string id)
        {
            var value = await _specialOfferService.GetSpecialOfferByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecialOffer(CreateSpecialOfferDto specialOfferDto)
        {
            await _specialOfferService.CreateSpecialOfferAsync(specialOfferDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSpecialOffer(UpdateSpecialOfferDto specialOfferDto)
        {
            await _specialOfferService.UpdateSpecialOfferAsync(specialOfferDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialOffer(string id)
        {
            await _specialOfferService.DeleteSpecialOfferByIdAsync(id);
            return Ok();
        }
    }
}
