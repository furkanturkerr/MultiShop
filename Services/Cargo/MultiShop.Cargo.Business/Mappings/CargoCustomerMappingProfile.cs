using AutoMapper;
using Cargo.Entities.Concrete;
using MultiShop.Cargo.Dto.CargoCustomerDtos;

namespace MultiShop.Cargo.Business.Mappings;

public class CargoCustomerMappingProfile : Profile
{
    public CargoCustomerMappingProfile()
    {
        CreateMap<CargoCustomer, ResultCargoCustomerDto>();
        CreateMap<CreateCargoCustomerDto, CargoCustomer>();
        CreateMap<UpdateCargoCustomerDto, CargoCustomer>();
    }
}
