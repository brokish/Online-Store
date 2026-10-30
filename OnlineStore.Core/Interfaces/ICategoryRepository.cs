using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task<bool> SaveChangesAsync();
}
