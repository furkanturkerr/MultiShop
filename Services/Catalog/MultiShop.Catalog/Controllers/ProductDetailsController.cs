using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Services.ProductDetailServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailService  _productDetailService;

        public ProductDetailsController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ProductDetailList()
        {
            var values = await _productDetailService.GetAllCategoriesAsync();
            return Ok(values);
        }

        [HttpGet("ProductDetailsByProductId")]
        [AllowAnonymous]
        public async Task<IActionResult> ProductDetailListByProductId(string productId)
        {
            var values = await _productDetailService.GetProductDetailByProductIdAsync(productId);
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ProductDetailById(string id)
        {
            var value = await _productDetailService.GetProductDetailByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProductDetail(CreateProductDetailDto productDetailDto)
        {
            await _productDetailService.CreateProductDetailAsync(productDetailDto);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductDetail(UpdateProductDetailDto productDetailDto)
        {
            await _productDetailService.UpdateProductDetailAsync(productDetailDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductDetail(string id)
        {
            await _productDetailService.DeleteProductDetailByIdAsync(id);
            return Ok();
        }
    }
}
