using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryRepository _repo;

    public InventoryController(IInventoryRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var item = await _repo.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryItem item)
    {
        if (item == null) return BadRequest();
        item.Id = Guid.NewGuid();
        await _repo.AddAsync(item);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] InventoryItem item)
    {
        if (item == null || id != item.Id) return BadRequest();
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return NotFound();
        existing.Quantity = item.Quantity;
        existing.IsDeleted = item.IsDeleted;
        await _repo.UpdateAsync(existing);
        return NoContent();
    }
}
