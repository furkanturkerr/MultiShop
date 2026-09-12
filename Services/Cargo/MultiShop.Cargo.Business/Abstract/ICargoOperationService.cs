using MultiShop.Cargo.Dto.CargoOperationDtos;

namespace MultiShop.Cargo.Business.Abstract;

public interface ICargoOperationService : IGenericService<ResultCargoOperationDto, CreateCargoOperationDto, UpdateCargoOperationDto>
{
    
}