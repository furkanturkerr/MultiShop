using MultiShop.Dtos.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface IOrderAddressService
{
    Task<List<ResultOrderAddressDto>> GetAllOrderAddressesAsync();
    Task<UpdateOrderAddressDto?> GetByIdOrderAddressAsync(int id);
    Task<int?> CreateOrderAddressAsync(CreateOrderAddressDto dto);
    Task<bool> UpdateOrderAddressAsync(UpdateOrderAddressDto dto);
    Task<bool> DeleteOrderAddressAsync(int id);
}
