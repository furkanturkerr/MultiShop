using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Services.ProductImageServices;
using MultiShop.Catalog.Services.ProductServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService  _productService;
        private readonly IProductImageService _productImageService;

        public ProductsController(IProductService productService, IProductImageService productImageService)
        {
            _productService = productService;
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var values = await _productService.GetAllProductsWithCategoryAsync();
            return Ok(values);
        }

        [HttpGet("CategoryId")]
        public async Task<IActionResult> ProductListByCategoryId(string categoryId)
        {
            var values = await _productService.GetProductsByCategoryIdAsync(categoryId);
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ProductById(string id)
        {
            var value = await _productService.GetProductByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
        {
            await _productService.CreateProductAsync(productDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto productDto)
        {
            await _productService.UpdateProductAsync(productDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductByIdAsync(id);
            return Ok();
        }
    }
}
