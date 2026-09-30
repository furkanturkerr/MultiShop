using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.FeatureSldierDtos;
using MultiShop.Catalog.Services.FeatureSliderServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureSliderController : ControllerBase
    {
        private readonly IFeatureSliderService  _sliderService;

        public FeatureSliderController(IFeatureSliderService sliderService)
        {
            _sliderService = sliderService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CategoryList()
        {
            var values = await _sliderService.GetAllFeatureSliderAsync();
            return Ok(values);
        }

        [HttpGet("status")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeatureSliderActiveStatus()
        {
            var values = await _sliderService.GetSliderByStatusAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> CategoryById(string id)
        {
            var value = await _sliderService.GetFeatureSliderByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(CreateFeatureSliderDto categoryDto)
        {
            await _sliderService.CreateFeatureSliderAsync(categoryDto);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(UpdateFeatureSliderDto categoryDto)
        {
            await _sliderService.UpdateFeatureSliderAsync(categoryDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _sliderService.DeleteFeatureSliderByIdAsync(id);
            return Ok();
        }
    }
}
