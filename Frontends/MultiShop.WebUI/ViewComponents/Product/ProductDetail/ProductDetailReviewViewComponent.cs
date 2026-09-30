using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CommentDtos;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailReviewViewComponent : ViewComponent
{
    private readonly ICommentService _commentService;

    public ProductDetailReviewViewComponent(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string id, CreateCommentDto? form = null)
    {
        var model = new ProductReviewViewModel
        {
            Form = form ?? new CreateCommentDto { ProductId = id }
        };

        try
        {
            var comments = await _commentService.GetCommentByProductIdAsync(id);
            model.Comments = comments.Where(x => x.IsApproved).OrderByDescending(x => x.CommentDate).ToList();
        }
        catch (HttpRequestException)
        {
            model.LoadError = "Yorum servisine şu anda ulaşılamıyor.";
        }
        catch (TaskCanceledException)
        {
            model.LoadError = "Yorumlar yüklenirken zaman aşımı oluştu.";
        }
        return View(model);
    }
}
