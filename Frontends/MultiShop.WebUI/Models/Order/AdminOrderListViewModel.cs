using MultiShop.Dtos.OrderDtos.OrderAddressDtos;
using MultiShop.Dtos.OrderDtos.OrderingDtos;

namespace MultiShop.WebUI.Models.Order;

public class AdminOrderListViewModel
{
    public List<ResultOrderingDto> Orders { get; set; } = [];
    public Dictionary<int, ResultOrderAddressDto> Addresses { get; set; } = [];
}
