using MultiShop.Dtos.OrderDtos.OrderingDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IOrderService
{
    Task<int?> CreateOrderingAsync(CreateOrderingDto dto);
    Task<List<ResultOrderByUserIdDto>> GetMyOrdersAsync();
    Task<List<ResultOrderingDto>> GetAllOrdersAsync();
    Task<ResultOrderingDto?> GetOrderDetailAsync(int id);
}
