using System.ComponentModel.DataAnnotations;

namespace MultiShop.Dtos.CommentDtos;

public class CreateCommentDto
{
    [Required(ErrorMessage = "Adınızı ve soyadınızı yazın.")]
    [StringLength(100, ErrorMessage = "Ad ve soyad en fazla 100 karakter olabilir.")]
    public string NameSurname { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta adresinizi yazın.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi yazın.")]
    [StringLength(254, ErrorMessage = "E-posta adresi en fazla 254 karakter olabilir.")]
    public string Email { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(120, ErrorMessage = "Yorum başlığı en fazla 120 karakter olabilir.")]
    public string? Comment { get; set; }

    [Required(ErrorMessage = "Yorumunuzu yazın.")]
    [StringLength(4000, ErrorMessage = "Yorum en fazla 4000 karakter olabilir.")]
    public string CommentDetail { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "1 ile 5 arasında bir puan seçin.")]
    public int Rating { get; set; }

    public DateTime CommentDate { get; set; }
    public bool IsApproved { get; set; }

    [Required(ErrorMessage = "Yorumun ait olduğu ürün bulunamadı.")]
    public string ProductId { get; set; } = string.Empty;
}
