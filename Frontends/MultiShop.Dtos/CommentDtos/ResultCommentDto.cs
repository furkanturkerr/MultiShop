namespace MultiShop.Dtos.CommentDtos;

public class ResultCommentDto
{
    public int UserCommentId { get; set; } 
    public string NameSurname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string CommentDetail { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CommentDate { get; set; }
    public bool IsApproved { get; set; }
}