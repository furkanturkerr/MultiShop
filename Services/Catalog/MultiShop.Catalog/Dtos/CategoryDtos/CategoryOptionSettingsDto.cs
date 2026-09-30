using System.ComponentModel.DataAnnotations;

namespace MultiShop.Catalog.Dtos.CategoryDtos;

public class CategoryOptionSettingsDto : IValidatableObject
{
    public List<string> OptionNames { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (OptionNames is null || OptionNames.Count > 6 ||
            OptionNames.Any(x => string.IsNullOrWhiteSpace(x) || x.Trim().Length > 40) ||
            OptionNames.Select(x => x.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count() != OptionNames.Count)
            yield return new ValidationResult("En fazla 6 farklı seçenek grubu girin. Grup adları en fazla 40 karakter olabilir.", new[] { nameof(OptionNames) });
    }
}
