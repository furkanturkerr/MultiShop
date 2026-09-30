using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> ProductList()
        {
            var values = await _productService.GetAllProductsWithCategoryAsync();
            return Ok(values);
        }

        [HttpGet("Filter")]
        [AllowAnonymous]
        public async Task<IActionResult> FilterProducts([FromQuery] ProductFilterDto filters, CancellationToken cancellationToken)
        {
            var values = await _productService.GetFilteredProductsAsync(filters, cancellationToken);
            return Ok(values);
        }

        [HttpGet("CategoryId")]
        [AllowAnonymous]
        public async Task<IActionResult> ProductListByCategoryId(string categoryId)
        {
            var values = await _productService.GetProductsByCategoryIdAsync(categoryId);
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ProductById(string id)
        {
            if (!MongoDB.Bson.ObjectId.TryParse(id, out _))
                return BadRequest("Geçersiz ürün kimliği.");
            var value = await _productService.GetProductByIdAsync(id);
            return value is null ? NotFound() : Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
        {
            var created = await _productService.CreateProductAsync(productDto);
            return created ? Ok() : BadRequest("Kategori veya ürün seçenekleri geçersiz. Yalnızca kategoride tanımlanan grupları kullanın.");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto productDto)
        {
            var updated = await _productService.UpdateProductAsync(productDto);
            return updated ? Ok() : BadRequest("Ürün, kategori veya seçenekler geçersiz. Yalnızca kategoride tanımlanan grupları kullanın.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductByIdAsync(id);
            return Ok();
        }
    }
}
