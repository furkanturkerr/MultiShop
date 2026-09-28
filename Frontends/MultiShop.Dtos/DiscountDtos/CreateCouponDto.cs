using System.ComponentModel.DataAnnotations;

namespace MultiShop.Dtos.DiscountDtos;

public class CreateCouponDto
{
    [Required(ErrorMessage = "Kupon kodu zorunludur.")]
    [StringLength(50, ErrorMessage = "Kupon kodu en fazla 50 karakter olabilir.")]
    public string Code { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "İndirim oranı 1 ile 100 arasında olmalıdır.")]
    public int Rate { get; set; }

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Geçerlilik tarihi zorunludur.")]
    public DateTime ValidDate { get; set; }
}
