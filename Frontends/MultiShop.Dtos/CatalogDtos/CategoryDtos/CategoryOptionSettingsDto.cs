using System.ComponentModel.DataAnnotations;

namespace MultiShop.Dtos.CatalogDtos.CategoryDtos;

public class CategoryOptionSettingsDto : IValidatableObject
{
    public List<string> OptionNames { get; set; } = new();

    [System.Text.Json.Serialization.JsonIgnore]
    public string OptionNamesText
    {
        get => string.Join(", ", OptionNames);
        set => OptionNames = (value ?? string.Empty)
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (OptionNames is null || OptionNames.Count > 6 ||
            OptionNames.Any(x => string.IsNullOrWhiteSpace(x) || x.Trim().Length > 40) ||
            OptionNames.Select(x => x.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count() != OptionNames.Count)
            yield return new ValidationResult("En fazla 6 farklı seçenek grubu girin. Grup adları en fazla 40 karakter olabilir.", new[] { nameof(OptionNames) });
    }
}
