using MultiShop.Dtos.CargoDtos.CargoCompanyDtos;

namespace MultiShop.WebUI.Services.Interfaces;

public interface ICargoCompanyService
{
    Task<List<ResultCargoCompanyDto>> GetAllAsync();
    Task<UpdateCargoCompanyDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreateCargoCompanyDto dto);
    Task<bool> UpdateAsync(UpdateCargoCompanyDto dto);
    Task<bool> DeleteAsync(int id);
}
