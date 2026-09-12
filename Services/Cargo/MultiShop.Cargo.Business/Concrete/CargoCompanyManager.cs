using AutoMapper;
using Cargo.Entities.Concrete;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.Dto.CargoCompanyDtos;

namespace MultiShop.Cargo.Business.Concrete;

public class CargoCompanyManager : ICargoCompanyService
{
    private readonly ICargoCompanyDal _cargoCompanyDal;
    private readonly IMapper _mapper;

    public CargoCompanyManager(ICargoCompanyDal cargoCompanyDal, IMapper mapper)
    {
        _cargoCompanyDal = cargoCompanyDal;
        _mapper = mapper;
    }

    public async Task<List<ResultCargoCompanyDto>> TGetAllAsync()
    {
        var values = await _cargoCompanyDal.GetAllAsync();
        return _mapper.Map<List<ResultCargoCompanyDto>>(values);
    }

    public async Task<ResultCargoCompanyDto?> TGetByIdAsync(int id)
    {
        var value = await _cargoCompanyDal.GetByIdAsync(id);
        return _mapper.Map<ResultCargoCompanyDto>(value);
    }

    public async Task TInsertAsync(CreateCargoCompanyDto dto)
    {
        var value = _mapper.Map<CargoCompany>(dto);
        await _cargoCompanyDal.AddAsync(value);
    }

    public async Task TUpdateAsync(UpdateCargoCompanyDto dto)
    {
        var value = _mapper.Map<CargoCompany>(dto);
        await _cargoCompanyDal.UpdateAsync(value);
    }

    public async Task TDeleteAsync(int id)
    {
        var value = await _cargoCompanyDal.GetByIdAsync(id);

        if (value is not null)
            await _cargoCompanyDal.DeleteAsync(value);
    }
}