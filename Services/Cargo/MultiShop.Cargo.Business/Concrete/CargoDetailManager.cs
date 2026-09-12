using AutoMapper;
using Cargo.Entities.Concrete;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.Dto.CargoDetailDtos;

namespace MultiShop.Cargo.Business.Concrete;

public class CargoDetailManager : ICargoDetailService
{
    private readonly ICargoDetailDal _cargoDetailDal;
    private readonly IMapper _mapper;

    public CargoDetailManager(ICargoDetailDal cargoDetailDal, IMapper mapper)
    {
        _cargoDetailDal = cargoDetailDal;
        _mapper = mapper;
    }

    public async Task<List<ResultCargoDetailDto>> TGetAllAsync()
    {
        var values = await _cargoDetailDal.GetAllAsync();
        return _mapper.Map<List<ResultCargoDetailDto>>(values);
    }

    public async Task<ResultCargoDetailDto?> TGetByIdAsync(int id)
    {
        var value = await _cargoDetailDal.GetByIdAsync(id);
        return _mapper.Map<ResultCargoDetailDto>(value);
    }

    public async Task TInsertAsync(CreateCargoDetailDto dto)
    {
        var value = _mapper.Map<CargoDetail>(dto);
        await _cargoDetailDal.AddAsync(value);
    }

    public async Task TUpdateAsync(UpdateCargoDetailDto dto)
    {
        var value = _mapper.Map<CargoDetail>(dto);
        await _cargoDetailDal.UpdateAsync(value);
    }

    public async Task TDeleteAsync(int id)
    {
        var value = await _cargoDetailDal.GetByIdAsync(id);

        if (value is not null)
            await _cargoDetailDal.DeleteAsync(value);
    }
}