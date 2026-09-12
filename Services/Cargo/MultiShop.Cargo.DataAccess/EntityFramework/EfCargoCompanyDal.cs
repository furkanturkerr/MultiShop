using Cargo.Entities.Concrete;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.DataAccess.Concrete;
using MultiShop.Cargo.DataAccess.Repository;

namespace MultiShop.Cargo.DataAccess.EntityFramework;

public class EfCargoCompanyDal : GenericRepository<CargoCompany>, ICargoCompanyDal
{
    public EfCargoCompanyDal(CargoContext cargoContext) : base(cargoContext)
    {
    }
}