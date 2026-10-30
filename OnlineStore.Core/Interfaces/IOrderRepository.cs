using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task<bool> SaveChangesAsync();
}
