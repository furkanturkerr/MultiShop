using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MultiShop.Dtos.CommentDtos;
using MultiShop.Dtos.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers;

public class ProductController : Controller
{
    private readonly ICommentService _commentService;
    private readonly IProductService _productService;

    public ProductController(ICommentService commentService, IProductService productService)
    {
        _commentService = commentService;
        _productService = productService;
    }

    // GET
    [HttpGet("product/category/{id}")]
    public async Task<IActionResult> Index(string id, [FromQuery] ProductFilterDto filters)
    {
        filters.CategoryId = id;
        return await ProductListAsync(filters);
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ProductFilterDto filters)
    {
        return await ProductListAsync(filters);
    }

    private async Task<IActionResult> ProductListAsync(ProductFilterDto filters)
    {
        if (!TryValidateModel(filters))
            return BadRequest(ModelState);

        var model = await _productService.GetProductListAsync(filters);
        return View("Index", model);
    }

    [HttpGet("product/detail/{id}")]
    public async Task<IActionResult> Detail(string id, bool reviews = false)
    {
        var product = await _productService.GetByIdProductAsync(id);
        if (product is null)
            return NotFound();
        ViewData["Product"] = product;
        ViewBag.Id = id;
        ViewData["ShowReviews"] = reviews;
        return View(new CreateCommentDto { ProductId = id });
    }

    [HttpPost("product/detail/{id}/comment")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(string id,
        [Bind("NameSurname,Email,Comment,CommentDetail,Rating,ProductId", Prefix = "Form")] CreateCommentDto form)
    {
        if (string.IsNullOrWhiteSpace(id) || !string.Equals(id, form.ProductId, StringComparison.Ordinal))
        {
            return BadRequest("Yorumun ait olduğu ürün doğrulanamadı.");
        }

        var product = await _productService.GetByIdProductAsync(id);
        if (product is null)
            return NotFound();
        ViewData["Product"] = product;
        ViewBag.Id = id;
        ViewData["ShowReviews"] = true;
        if (!ModelState.IsValid)
        {
            return View("Detail", form);
        }

        form.NameSurname = form.NameSurname.Trim();
        form.Email = form.Email.Trim();
        form.Comment = form.Comment?.Trim() ?? string.Empty;
        form.CommentDetail = form.CommentDetail.Trim();
        form.ImageUrl = string.Empty;
        form.CommentDate = DateTime.UtcNow;
        form.IsApproved = false;

        try
        {
            if (await _commentService.CreateCommentAsync(form))
            {
                TempData["CommentSuccessMessage"] = "Yorumun alındı. Onaylandıktan sonra burada yayımlanacak.";
                return RedirectToAction(nameof(Detail), "Product", new { id, reviews = true }, "tab-pane-3");
            }

            ModelState.AddModelError(string.Empty, "Yorum gönderilemedi. Bilgilerini kontrol edip tekrar dene.");
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "Yorum servisine ulaşılamıyor. Lütfen daha sonra tekrar dene.");
        }
        catch (TaskCanceledException)
        {
            ModelState.AddModelError(string.Empty, "Yorum isteği zaman aşımına uğradı. Tekrar göndermeden önce kaydını kontrol et.");
        }
        return View("Detail", form);
    }
}
