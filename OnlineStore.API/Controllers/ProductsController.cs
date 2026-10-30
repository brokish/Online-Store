using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.DTOs;
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

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
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
    
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();

        return Ok(products);
    }
}