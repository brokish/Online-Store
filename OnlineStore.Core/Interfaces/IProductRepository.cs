using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync(); 
    Task<bool> SaveChangesAsync();
}