using Cargo.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.DataAccess.Concrete;
using MultiShop.Cargo.DataAccess.Repository;

namespace MultiShop.Cargo.DataAccess.EntityFramework;

public class EfCargoCustomerDal : GenericRepository<CargoCustomer>, ICargoCustomerDal
{
    private readonly CargoContext _cargoContext;

    public EfCargoCustomerDal(CargoContext cargoContext) : base(cargoContext)
    {
        _cargoContext = cargoContext;
    }

    public Task<CargoCustomer?> GetByUserCustomerIdAsync(string userCustomerId)
    {
        return _cargoContext.CargoCustomers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.UserCustomerId == userCustomerId);
    }
}
