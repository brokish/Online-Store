using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Requests;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "პროდუქტი ვერ მოიძებნა!" });

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreate dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description ?? string.Empty,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId
        };

        await _productService.CreateProductAsync(product);
        return Ok(new { message = "პროდუქტი წარმატებით დაემატა!", product });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductCreate dto)
    {
        var updatedProduct = new Product
        {
            Name = dto.Name,
            Description = dto.Description ?? string.Empty,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId
        };

        var result = await _productService.UpdateProductAsync(id, updatedProduct);
        if (!result)
            return NotFound(new { message = "პროდუქტი ვერ მოიძებნა!" });

        return Ok(new { message = "პროდუქტი წარმატებით განახლდა!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (!result)
            return NotFound(new { message = "პროდუქტი ვერ მოიძებნა!" });

        return Ok(new { message = "პროდუქტი წარმატებით წაიშალა!" });
    }
}