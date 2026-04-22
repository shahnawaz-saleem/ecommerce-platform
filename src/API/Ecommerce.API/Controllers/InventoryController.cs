using Inventory.Domain.Entities;
using MediatR;
using Inventory.Application.Queries.GetInventoryById;
using Inventory.Application.Queries.GetInventories;
using Inventory.Application.Commands.CreateInventoryItem;
using Inventory.Application.Commands.UpdateInventoryItem;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var item = await _mediator.Send(new GetInventoryByIdQuery(id));
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
    {
        var items = await _mediator.Send(new GetInventoriesQuery(page, pageSize));
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInventoryItemCommand command)
    {
        if (command == null) return BadRequest();
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInventoryItemCommand command)
    {
        if (command == null || id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }
}
