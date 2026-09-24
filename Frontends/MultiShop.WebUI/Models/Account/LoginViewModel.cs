using System.ComponentModel.DataAnnotations;

namespace MultiShop.WebUI.Models.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "E-posta adresini gir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi gir.")]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifreni gir.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}
