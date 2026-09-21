using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.BrandDtos;
using MultiShop.Catalog.Services.BrandServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService  _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> BrandList()
        {
            var values = await _brandService.GetAllBrandAsync();
            return Ok(values);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetBrandActiveStatus()
        {
            var values = await _brandService.GetBrandByStatusAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BrandById(string id)
        {
            var value = await _brandService.GetBrandByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrandDto brandDto)
        {
            await _brandService.CreateBrandAsync(brandDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBrand(UpdateBrandDto brandDto)
        {
            await _brandService.UpdateBrandAsync(brandDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            await _brandService.DeleteBrandByIdAsync(id);
            return Ok();
        }
    }
}
