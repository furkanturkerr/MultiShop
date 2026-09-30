namespace MultiShop.Catalog.Entities;

public class ProductOption
{
    public string Name { get; set; } = string.Empty;
    public List<string> Values { get; set; } = new();
}
