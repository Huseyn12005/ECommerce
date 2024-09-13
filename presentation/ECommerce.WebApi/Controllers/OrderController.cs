using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Concretes;
using ECommerce.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IWriteOrderRepository _writeOrderRepo;
    private readonly IReadOrderRepository _readOrderRepo;

    public OrderController(IWriteOrderRepository writeOrderRepo, IReadOrderRepository readOrderRepo)
    {
        _writeOrderRepo = writeOrderRepo;
        _readOrderRepo = readOrderRepo;
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderVM orderVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = new Order
        {
            OrderNumber = orderVM.OrderNumber,
            OrderDate = orderVM.OrderDate,
            OrderNote = orderVM.OrderNote,
            Total = orderVM.Total,
            CustomerId = orderVM.CustomerId,
            Products = orderVM.Products.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId
            }).ToList()
        };

        await _writeOrderRepo.AddAsync(order);
        await _writeOrderRepo.SaveChangeAsync();

        return StatusCode(201);
    }

    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _readOrderRepo.GetAllAsync();
        if (orders == null || !orders.Any())
            return NotFound("No orders found");

        var orderVMs = orders.Select(o => new OrderVM
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            OrderDate = o.OrderDate,
            OrderNote = o.OrderNote,
            Total = o.Total,
            CustomerId = o.CustomerId,
            Products = o.Products.Select(p => new ProductVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name
            }).ToList()
        }).ToList();

        return Ok(orderVMs);
    }

    [HttpGet("GetOrder/{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _readOrderRepo.GetByIdAsync(id);
        if (order == null)
            return NotFound("Order not found");

        var orderVM = new OrderVM
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            OrderNote = order.OrderNote,
            Total = order.Total,
            CustomerId = order.CustomerId,
            Products = order.Products.Select(p => new ProductVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name
            }).ToList()
        };

        return Ok(orderVM);
    }

    [HttpPut("UpdateOrder/{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderVM orderVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = await _readOrderRepo.GetByIdAsync(id);
        if (order == null)
            return NotFound("Order not found");

        order.OrderNumber = orderVM.OrderNumber;
        order.OrderDate = orderVM.OrderDate;
        order.OrderNote = orderVM.OrderNote;
        order.Total = orderVM.Total;
        order.CustomerId = orderVM.CustomerId;

        order.Products = orderVM.Products.Select(p => new Product
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            Stock = p.Stock,
            ImageUrl = p.ImageUrl,
            CategoryId = p.CategoryId
        }).ToList();

        await _writeOrderRepo.UpdateAsync(order);
        await _writeOrderRepo.SaveChangeAsync();

        return Ok();
    }

    [HttpDelete("DeleteOrder/{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _readOrderRepo.GetByIdAsync(id);
        if (order == null)
            return NotFound("Order not found");

        await _writeOrderRepo.DeleteAsync(order);
        await _writeOrderRepo.SaveChangeAsync();

        return NoContent();
    }
}
