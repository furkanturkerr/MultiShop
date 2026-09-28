using System.ComponentModel.DataAnnotations;
using MultiShop.Dtos.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Models.Order;

public class OrderCheckoutViewModel
{
    public CreateOrderAddressDto Address { get; set; } = new();
    public List<ResultOrderAddressDto> SavedAddresses { get; set; } = [];
    public int SelectedAddressId { get; set; }

    [Required(ErrorMessage = "Kart üzerindeki adı yazmalısınız.")]
    public string CardHolderName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kart numarası zorunludur.")]
    [CreditCard(ErrorMessage = "Geçerli bir kart numarası yazmalısınız.")]
    public string CardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Son kullanma ayını seçmelisiniz.")]
    [RegularExpression("^(0[1-9]|1[0-2])$", ErrorMessage = "Geçerli bir ay seçmelisiniz.")]
    public string ExpiryMonth { get; set; } = string.Empty;

    [Required(ErrorMessage = "Son kullanma yılını seçmelisiniz.")]
    [RegularExpression("^\\d{2}$", ErrorMessage = "Geçerli bir yıl seçmelisiniz.")]
    public string ExpiryYear { get; set; } = string.Empty;

    [Required(ErrorMessage = "CVV zorunludur.")]
    [RegularExpression("^\\d{3,4}$", ErrorMessage = "CVV 3 veya 4 rakam olmalıdır.")]
    public string Cvv { get; set; } = string.Empty;

    [Range(typeof(bool), "true", "true", ErrorMessage = "Ön bilgilendirme koşullarını kabul etmelisiniz.")]
    public bool AcceptTerms { get; set; }
}
