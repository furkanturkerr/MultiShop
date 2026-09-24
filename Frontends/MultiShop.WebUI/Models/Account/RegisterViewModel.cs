using System.ComponentModel.DataAnnotations;

namespace MultiShop.WebUI.Models.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Adını gir.")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyadını gir.")]
    [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir.")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta adresini gir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi gir.")]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bir şifre belirle.")]
    [StringLength(128, MinimumLength = 6, ErrorMessage = "Şifre 6–128 karakter arasında olmalı.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "Şifre büyük harf, küçük harf, rakam ve özel karakter içermeli.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifreni tekrar gir.")]
    [Compare(nameof(Password), ErrorMessage = "Şifreler aynı olmalı.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Range(typeof(bool), "true", "true", ErrorMessage = "Devam etmek için kullanım koşullarını kabul et.")]
    public bool AcceptTerms { get; set; }

    public string? ReturnUrl { get; set; }
}
