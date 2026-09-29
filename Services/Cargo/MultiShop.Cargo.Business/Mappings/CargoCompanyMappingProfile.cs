using AutoMapper;
using Cargo.Entities.Concrete;
using MultiShop.Cargo.Dto.CargoCompanyDtos;

namespace MultiShop.Cargo.Business.Mappings;

public class CargoCompanyMappingProfile : Profile
{
    public CargoCompanyMappingProfile()
    {
        CreateMap<CargoCompany, ResultCargoCompanyDto>();
        CreateMap<CreateCargoCompanyDto, CargoCompany>();
        CreateMap<UpdateCargoCompanyDto, CargoCompany>();
    }
}
