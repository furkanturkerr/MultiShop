using Cargo.Entities.Concrete;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.DataAccess.Concrete;
using MultiShop.Cargo.DataAccess.Repository;

namespace MultiShop.Cargo.DataAccess.EntityFramework;

public class EfCargoOperationDal : GenericRepository<CargoOperation>, ICargoOperationDal
{
    public EfCargoOperationDal(CargoContext cargoContext) : base(cargoContext)
    {
    }
}