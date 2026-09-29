using Cargo.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace MultiShop.Cargo.DataAccess.Concrete;

public class CargoContext : DbContext
{
    public CargoContext(DbContextOptions<CargoContext> options) : base(options)
    {
    }

    public DbSet<CargoCompany> CargoCompanies { get; set; }
    public DbSet<CargoCustomer> CargoCustomers { get; set; }
    public DbSet<CargoDetail> CargoDetails { get; set; }
    public DbSet<CargoOperation> CargoOperations { get; set; }
}
