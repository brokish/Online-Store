using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.API.DTOs;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Interfaces;
using OnlineStore.Infrastructure.Data;

namespace OnlineStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly OnlineStoreDbContext _context;

    public OrdersController(IOrderService orderService, OnlineStoreDbContext context)
    {
        _orderService = orderService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound(new { message = "შეკვეთა ვერ მოიძებნა!" });

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        if (dto.CustomerId <= 0)
            return BadRequest(new { message = "CustomerId სავალდებულოა." });

        var customerExists = await _context.Customers.AnyAsync(c => c.Id == dto.CustomerId);
        if (!customerExists)
            return BadRequest(new { message = "მოცემული CustomerId ვერ მოიძებნა." });

        if (dto.Items == null || dto.Items.Count == 0)
            return BadRequest(new { message = "შეკვეთაში უნდა იყოს მინიმუმ ერთი პროდუქტი." });

        var order = new Order
        {
            CustomerId = dto.CustomerId,
            Status = "Pending",
            Items = new List<OrderItem>()
        };

        foreach (var itemDto in dto.Items)
        {
            if (itemDto.Quantity <= 0)
                return BadRequest(new { message = "პროდუქტის რაოდენობა უნდა იყოს 1 ან მეტი." });

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == itemDto.ProductId);
            if (product == null)
                return BadRequest(new { message = $"პროდუქტი #{itemDto.ProductId} ვერ მოიძებნა." });

            if (product.StockQuantity < itemDto.Quantity)
                return BadRequest(new { message = $"პროდუქტი #{product.Id} არ არის საკმარისი მარაგში." });

            order.Items.Add(new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        var createdOrder = await _orderService.CreateOrderAsync(order);
        if (createdOrder == null)
            return BadRequest(new { message = "შეკვეთა ვერ შეიქმნა. შეამოწმეთ მარაგები და პროდუქტები." });

        return Ok(new { message = "შეკვეთა წარმატებით შეიქმნა!", order = createdOrder });
    }
}
