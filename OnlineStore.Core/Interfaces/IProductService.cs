using OnlineStore.Core.Entities;

namespace OnlineStore.Core.Interfaces;

public interface IProductService
{
    Task CreateProductAsync(Product product);
    Task<IEnumerable<Product>> GetAllProductsAsync();
}