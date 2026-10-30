using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order?> CreateOrderAsync(Order order);
}
