using System.ComponentModel.DataAnnotations;

namespace MultiShop.Dtos.OrderDtos.OrderAddressDtos;

public class CreateOrderAddressDto
{
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi yazmalısınız.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon alanı zorunludur.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ülke alanı zorunludur.")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "İlçe alanı zorunludur.")]
    public string District { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şehir alanı zorunludur.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres alanı zorunludur.")]
    public string Detail1 { get; set; } = string.Empty;
    public string Detail2 { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
