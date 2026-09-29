using System.ComponentModel.DataAnnotations;

namespace MultiShop.Dtos.CargoDtos.CargoCompanyDtos;

public class UpdateCargoCompanyDto
{
    public int CargoCompanyId { get; set; }

    [Required(ErrorMessage = "Kargo şirketi adı zorunludur.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Şirket adı 2-100 karakter arasında olmalıdır.")]
    public string CompanyName { get; set; } = string.Empty;
}
