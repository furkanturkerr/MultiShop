namespace MultiShop.Discount.Dtos;

public class CreateCouponDto
{
    public string Code { get; set; } = string.Empty;
    public int Rate { get; set; }
    public bool IsActive { get; set; }
    public DateTime ValidDate { get; set; }
}