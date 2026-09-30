using System.ComponentModel.DataAnnotations;

namespace MultiShop.Catalog.Dtos.ProductDtos;

public class ProductFilterDto
{
    [StringLength(200)]
    public string? Q { get; set; }

    [RegularExpression("^[a-fA-F0-9]{24}$")]
    public string? CategoryId { get; set; }

    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string Sort { get; set; } = "name-asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
