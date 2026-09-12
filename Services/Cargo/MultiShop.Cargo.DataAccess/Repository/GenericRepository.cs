using Microsoft.EntityFrameworkCore;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.DataAccess.Concrete;

namespace MultiShop.Cargo.DataAccess.Repository;

public class GenericRepository<T> : IGenericDal<T> where T : class
{
    private readonly CargoContext _cargoContext;

    public GenericRepository(CargoContext cargoContext)
    {
        _cargoContext = cargoContext;
    }

    public async Task<List<T>> GetAllAsync()
    {
        var values = await _cargoContext.Set<T>().ToListAsync();
        return values;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var values = await _cargoContext.Set<T>().FindAsync(id);
        return values;
    }

    public async Task AddAsync(T entity)
    {
        _cargoContext.Set<T>().Add(entity);
        await _cargoContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _cargoContext.Set<T>().Update(entity);
        await _cargoContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _cargoContext.Set<T>().Remove(entity);
        await _cargoContext.SaveChangesAsync();
    }
}