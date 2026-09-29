using AutoMapper;
using Cargo.Entities.Concrete;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.Dto.CargoCustomerDtos;

namespace MultiShop.Cargo.Business.Concrete;

public class CargoCustomerManager : ICargoCustomerService
{
    private readonly ICargoCustomerDal _cargoCustomerDal;
    private readonly IMapper _mapper;

    public CargoCustomerManager(ICargoCustomerDal cargoCustomerDal, IMapper mapper)
    {
        _cargoCustomerDal = cargoCustomerDal;
        _mapper = mapper;
    }

    public async Task<List<ResultCargoCustomerDto>> TGetAllAsync()
    {
        var values = await _cargoCustomerDal.GetAllAsync();
        return _mapper.Map<List<ResultCargoCustomerDto>>(values);
    }

    public async Task<ResultCargoCustomerDto?> TGetByIdAsync(int id)
    {
        var value = await _cargoCustomerDal.GetByIdAsync(id);
        return _mapper.Map<ResultCargoCustomerDto>(value);
    }

    public async Task<ResultCargoCustomerDto?> TGetByUserCustomerIdAsync(string userCustomerId)
    {
        var value = await _cargoCustomerDal.GetByUserCustomerIdAsync(userCustomerId);
        return value is null ? null : _mapper.Map<ResultCargoCustomerDto>(value);
    }

    public async Task TInsertAsync(CreateCargoCustomerDto dto)
    {
        var value = _mapper.Map<CargoCustomer>(dto);
        await _cargoCustomerDal.AddAsync(value);
    }

    public async Task TUpdateAsync(UpdateCargoCustomerDto dto)
    {
        var value = _mapper.Map<CargoCustomer>(dto);
        await _cargoCustomerDal.UpdateAsync(value);
    }

    public async Task TDeleteAsync(int id)
    {
        var value = await _cargoCustomerDal.GetByIdAsync(id);

        if (value is not null)
            await _cargoCustomerDal.DeleteAsync(value);
    }
}
