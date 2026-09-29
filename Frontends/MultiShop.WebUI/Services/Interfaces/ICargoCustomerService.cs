using MultiShop.Dtos.CargoDtos.CargoCustomerDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface ICargoCustomerService
{
    Task<List<ResultCargoCustomerDto>> GetAllAsync();
    Task<ResultCargoCustomerDto?> GetMyCargoCustomerAsync();
}
