using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MultiShop.Dtos.CommentDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers;

public class ProductController : Controller
{
    private readonly ICommentService _commentService;

    public ProductController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    // GET
    [ Route("product/category/{id}")]
    public IActionResult Index(string id)
    {
        ViewBag.Id = id;
        return View();
    }

    [HttpGet("product/detail/{id}")]
    public IActionResult Detail(string id, bool reviews = false)
    {
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
