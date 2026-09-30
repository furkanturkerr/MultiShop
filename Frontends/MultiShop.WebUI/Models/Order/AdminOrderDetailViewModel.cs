using MultiShop.Dtos.OrderDtos.OrderAddressDtos;
using MultiShop.Dtos.OrderDtos.OrderingDtos;

namespace MultiShop.WebUI.Models.Order;

public class AdminOrderDetailViewModel
{
    public ResultOrderingDto Order { get; set; } = new();
    public UpdateOrderAddressDto? Address { get; set; }
}
