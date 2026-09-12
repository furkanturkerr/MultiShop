using Cargo.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace MultiShop.Cargo.DataAccess.Concrete;

public class CargoContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1995;Database=MultiShopCargoDb;User Id=sa;Password=Furkan12*;TrustServerCertificate=True");
    }
    
    public DbSet<CargoCompany> CargoCompanies { get; set; }
    public DbSet<CargoCustomer> CargoCustomers { get; set; }
    public DbSet<CargoDetail> CargoDetails { get; set; }
    public DbSet<CargoOperation> CargoOperations { get; set; }
}