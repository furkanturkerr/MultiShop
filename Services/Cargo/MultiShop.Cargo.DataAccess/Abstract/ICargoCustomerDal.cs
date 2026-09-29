using Cargo.Entities.Concrete;

namespace MultiShop.Cargo.DataAccess.Abstract;

public interface ICargoCustomerDal : IGenericDal<CargoCustomer>
{
    Task<CargoCustomer?> GetByUserCustomerIdAsync(string userCustomerId);
}
