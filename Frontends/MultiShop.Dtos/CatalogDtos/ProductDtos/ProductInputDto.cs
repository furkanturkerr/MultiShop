using System.ComponentModel.DataAnnotations;

namespace MultiShop.Dtos.CatalogDtos.ProductDtos;

public abstract class ProductInputDto : IValidatableObject
{
    public string ProductName { get; set; }

    public string ProductDescription { get; set; }

    public decimal ProductPrice { get; set; }

    public string ProductImageUrl { get; set; }

    public string CategoryId { get; set; }

    public List<ProductOptionDto> Options { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Options is null || Options.Count > 6 || Options.Any(x => x is null) ||
            Options.Select(x => (x.Name ?? string.Empty).Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count() != Options.Count)
        {
            yield return new ValidationResult(
                "En fazla 6 farklı seçenek grubu eklenebilir.",
                new[] { nameof(Options) });
        }
    }
}
