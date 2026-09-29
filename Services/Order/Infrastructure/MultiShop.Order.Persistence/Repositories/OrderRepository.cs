using Microsoft.EntityFrameworkCore;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using MultiShop.Order.Persistence.Context;

namespace MultiShop.Order.Persistence.Repositories;

public class OrderRepository : Repository<Ordering>, IOrderRepository
{
    private readonly OrderContext _orderContext;
    
    public OrderRepository(OrderContext orderContext) : base(orderContext)
    {
        _orderContext = orderContext;
    }

    public async Task<List<Ordering>> GetOrdersByUserId(string userId)
    {
        return await _orderContext.Orderings
            .AsNoTracking()
            .Include(x => x.OrderDetails)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync();
    }
}
