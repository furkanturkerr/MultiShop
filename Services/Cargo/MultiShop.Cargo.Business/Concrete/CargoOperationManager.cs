using AutoMapper;
using Cargo.Entities.Concrete;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.Dto.CargoOperationDtos;

namespace MultiShop.Cargo.Business.Concrete;

public class CargoOperationManager : ICargoOperationService
{
    private readonly ICargoOperationDal _cargoOperationDal;
    private readonly IMapper _mapper;

    public CargoOperationManager(ICargoOperationDal cargoOperationDal, IMapper mapper)
    {
        _cargoOperationDal = cargoOperationDal;
        _mapper = mapper;
    }

    public async Task<List<ResultCargoOperationDto>> TGetAllAsync()
    {
        var values = await _cargoOperationDal.GetAllAsync();
        return _mapper.Map<List<ResultCargoOperationDto>>(values);
    }

    public async Task<ResultCargoOperationDto?> TGetByIdAsync(int id)
    {
        var value = await _cargoOperationDal.GetByIdAsync(id);
        return _mapper.Map<ResultCargoOperationDto>(value);
    }

    public async Task TInsertAsync(CreateCargoOperationDto dto)
    {
        var value = _mapper.Map<CargoOperation>(dto);
        await _cargoOperationDal.AddAsync(value);
    }

    public async Task TUpdateAsync(UpdateCargoOperationDto dto)
    {
        var value = _mapper.Map<CargoOperation>(dto);
        await _cargoOperationDal.UpdateAsync(value);
    }

    public async Task TDeleteAsync(int id)
    {
        var value = await _cargoOperationDal.GetByIdAsync(id);

        if (value is not null)
            await _cargoOperationDal.DeleteAsync(value);
    }
}