using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Interfaces;

public interface IOrderRepository : IRepository<Ordering>
{
    Task<List<Ordering>> GetOrdersByUserId(string userId);
    Task<Ordering?> GetOrderWithDetailsAsync(int id);
}
