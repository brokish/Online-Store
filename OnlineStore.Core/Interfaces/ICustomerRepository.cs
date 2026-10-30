using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task<bool> SaveChangesAsync();
}
