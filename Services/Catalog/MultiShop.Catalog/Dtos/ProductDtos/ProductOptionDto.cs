using System.ComponentModel.DataAnnotations;

namespace MultiShop.Catalog.Dtos.ProductDtos;

public class ProductOptionDto : IValidatableObject
{
    [Required(ErrorMessage = "Seçenek adı gerekli.")]
    [StringLength(40)]
    public string Name { get; set; } = string.Empty;
    public List<string> Values { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Values is null || Values.Count > 30 ||
            Values.Any(x => string.IsNullOrWhiteSpace(x) || x.Length > 60) ||
            Values.Distinct(StringComparer.OrdinalIgnoreCase).Count() != Values.Count)
            yield return new ValidationResult("Bir grupta en fazla 30 farklı değer olabilir; değerler en fazla 60 karakter olabilir.", new[] { nameof(Values) });
    }
}
