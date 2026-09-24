using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CommentDtos;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Infrastructure.Gateway;

namespace MultiShop.WebUI.ViewComponents.Product.ProductDetail;

public class ProductDetailReviewViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductDetailReviewViewComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string id, CreateCommentDto? form = null)
    {
        var model = new ProductReviewViewModel
        {
            Form = form ?? new CreateCommentDto { ProductId = id }
        };

        if (HttpContext.User.Identity?.IsAuthenticated != true)
        {
            model.LoadError = "Yorumları görüntülemek için giriş yap.";
            return View(model);
        }

        try
        {
            var client = _httpClientFactory.CreateClient("GatewayApi");
            using var response = await client.GetAsync($"comment/Comments/CommentByProductId?productId={Uri.EscapeDataString(id)}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    var comments = await response.Content.ReadFromJsonAsync<List<ResultCommentDto>>() ?? new();
                    model.Comments = comments.Where(x => x.IsApproved).OrderByDescending(x => x.CommentDate).ToList();
                }
            }
            else
            {
                model.LoadError = "Yorumlar şu anda yüklenemiyor. Lütfen daha sonra tekrar dene.";
            }
        }
        catch (HttpRequestException)
        {
            model.LoadError = "Yorum servisine şu anda ulaşılamıyor.";
        }
        catch (TaskCanceledException)
        {
            model.LoadError = "Yorumlar yüklenirken zaman aşımı oluştu.";
        }
        catch (GatewayApiException exception) when (exception.StatusCode is not
            (System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden))
        {
            model.LoadError = "Yorumlar şu anda yüklenemiyor. Lütfen daha sonra tekrar dene.";
        }

        return View(model);
    }
}
