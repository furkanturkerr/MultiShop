using Cargo.Entities.Concrete;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.DataAccess.Concrete;
using MultiShop.Cargo.DataAccess.Repository;

namespace MultiShop.Cargo.DataAccess.EntityFramework;

public class EfCargoDetailDal : GenericRepository<CargoDetail>, ICargoDetailDal
{
    public EfCargoDetailDal(CargoContext cargoContext) : base(cargoContext)
    {
    }
}