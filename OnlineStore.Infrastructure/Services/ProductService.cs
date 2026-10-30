using OnlineStore.Core.Entities;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    // <--- ახალი
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task CreateProductAsync(Product product)
    {
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();
    }

    public async Task<bool> UpdateProductAsync(int id, Product updatedProduct)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) return false;

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Description = updatedProduct.Description;
        existingProduct.Price = updatedProduct.Price;
        existingProduct.StockQuantity = updatedProduct.StockQuantity;
        existingProduct.CategoryId = updatedProduct.CategoryId;

        _productRepository.Update(existingProduct);
        return await _productRepository.SaveChangesAsync();
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return false;

        _productRepository.Delete(product);
        return await _productRepository.SaveChangesAsync();
    }
}