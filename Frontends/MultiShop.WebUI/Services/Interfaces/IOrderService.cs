using MultiShop.Dtos.OrderDtos.OrderDetailDtos;
using MultiShop.Dtos.OrderDtos.OrderingDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IOrderService
{
    Task<int?> CreateOrderingAsync(CreateOrderingDto dto);
    Task<bool> CreateOrderDetailAsync(CreateOrderDetailDto dto);
}
