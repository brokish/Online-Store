using Microsoft.EntityFrameworkCore;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Interfaces;
using OnlineStore.Infrastructure.Data;

namespace OnlineStore.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly OnlineStoreDbContext _context;

    public OrderService(IOrderRepository orderRepository, OnlineStoreDbContext context)
    {
        _orderRepository = orderRepository;
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<Order?> CreateOrderAsync(Order order)
    {
        if (order.Items == null || !order.Items.Any())
        {
            return null;
        }

        foreach (var item in order.Items)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
            if (product == null)
            {
                return null;
            }

            if (product.StockQuantity < item.Quantity)
            {
                return null;
            }

            item.UnitPrice = product.Price;
            product.StockQuantity -= item.Quantity;
        }

        order.OrderDate = DateTime.UtcNow;
        order.Status = "Pending";
        order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        return order;
    }
}
