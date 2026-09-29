using MultiShop.Cargo.Dto.CargoCustomerDtos;

namespace MultiShop.Cargo.Business.Abstract;

public interface ICargoCustomerService : IGenericService<ResultCargoCustomerDto, CreateCargoCustomerDto, UpdateCargoCustomerDto>
{
    Task<ResultCargoCustomerDto?> TGetByUserCustomerIdAsync(string userCustomerId);
}
