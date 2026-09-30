using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Services.ProductImageServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService  _productImageService;

        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ProductImageList()
        {
            var values = await _productImageService.GetAllCategoriesAsync();
            return Ok(values);
        }

        [HttpGet("ProductImagesByProductId")]
        [AllowAnonymous]
        public async Task<IActionResult> ProductImageListByProductId(string productId)
        {
            var values = await _productImageService.GetProductImageByProductIdAsync(productId);
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ProductImageById(string id)
        {
            var value = await _productImageService.GetProductImageByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProductImage(CreateProductImageDto productImageDto)
        {
            await _productImageService.CreateProductImageAsync(productImageDto);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductImage(UpdateProductImageDto productImageDto)
        {
            await _productImageService.UpdateProductImageAsync(productImageDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductImage(string id)
        {
            await _productImageService.DeleteProductImageByIdAsync(id);
            return Ok();
        }
    }
}
