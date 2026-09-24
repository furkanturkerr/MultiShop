using MultiShop.Dtos.CommentDtos;

namespace MultiShop.WebUI.Models;

public class ProductReviewViewModel
{
    public List<ResultCommentDto> Comments { get; set; } = new();
    public CreateCommentDto Form { get; set; } = new();
    public string? LoadError { get; set; }
}
