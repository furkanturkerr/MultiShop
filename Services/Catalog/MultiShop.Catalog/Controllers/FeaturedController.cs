using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.FeaturedDtos;
using MultiShop.Catalog.Services.FeaturedServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturedController : ControllerBase
    {
        private readonly IFeaturedService  _featuredService;

        public FeaturedController(IFeaturedService featuredService)
        {
            _featuredService = featuredService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FeaturedList()
        {
            var values = await _featuredService.GetAllFeaturedAsync();
            return Ok(values);
        }

        [HttpGet("status")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedActiveStatus()
        {
            var values = await _featuredService.GetFeaturedByStatusAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> FeaturedById(string id)
        {
            var value = await _featuredService.GetFeaturedByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateFeatured(CreateFeaturedDto featuredDto)
        {
            await _featuredService.CreateFeaturedAsync(featuredDto);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFeatured(UpdateFeaturedDto featuredDto)
        {
            await _featuredService.UpdateFeaturedAsync(featuredDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeatured(string id)
        {
            await _featuredService.DeleteFeaturedByIdAsync(id);
            return Ok();
        }
    }
}
