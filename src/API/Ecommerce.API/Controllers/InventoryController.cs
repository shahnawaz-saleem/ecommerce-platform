using Inventory.Domain.Entities;
using MediatR;
using Inventory.Application.Queries.GetInventoryById;
using Inventory.Application.Queries.GetInventories;
using Inventory.Application.Commands.CreateInventoryItem;
using Inventory.Application.Commands.UpdateInventoryItem;
using Inventory.Application.Commands.AddStock;
using Inventory.Application.Commands.ReserveStock;
using Inventory.Application.Commands.ConfirmReservation;
using Inventory.Application.Commands.ReleaseReservation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Roles = "SuperAdmin")]
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

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var dto = await _mediator.Send(new Inventory.Application.Queries.GetInventoryDashboard.GetInventoryDashboardQuery(), cancellationToken);
        return Ok(dto);
    }

    [HttpGet("alerts")]
    public async Task<IActionResult> GetLowStockAlerts([FromQuery] int threshold = 5, CancellationToken cancellationToken = default)
    {
        var alerts = await _mediator.Send(new Inventory.Application.Queries.GetLowStockAlerts.GetLowStockAlertsQuery(threshold), cancellationToken);
        return Ok(alerts);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInventoryItemCommand command)
    {
        if (command == null) return BadRequest();
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateInventoryItemCommand command, CancellationToken cancellationToken)
    {
        if (command == null || id != command.Id) return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{productId}/add")]
    public async Task<IActionResult> AddStock(Guid productId, AddStockCommand command, CancellationToken cancellationToken)
    {
        if (command == null || productId != command.ProductId) return BadRequest();

        var id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPost("{productId}/reserve")]
    public async Task<IActionResult> ReserveStock(Guid productId, ReserveStockCommand command, CancellationToken cancellationToken)
    {
        if (command == null || productId != command.ProductId) return BadRequest();

        var ok = await _mediator.Send(command, cancellationToken);

        if (!ok) return BadRequest("Insufficient stock or inventory not found");

        return NoContent();
    }

    [HttpPost("{productId}/confirm")]
    public async Task<IActionResult> ConfirmReservation(Guid productId, ConfirmReservationCommand command, CancellationToken cancellationToken)
    {
        if (command == null || productId != command.ProductId) return BadRequest();

        var ok = await _mediator.Send(command, cancellationToken);

        if (!ok) return BadRequest("Confirm failed");

        return NoContent();
    }

    [HttpPost("{productId}/release")]
    public async Task<IActionResult> ReleaseReservation(Guid productId, ReleaseReservationCommand command, CancellationToken cancellationToken)
    {
        if (command == null || productId != command.ProductId) return BadRequest();

        var ok = await _mediator.Send(command, cancellationToken);

        if (!ok) return BadRequest("Release failed");

        return NoContent();
    }
}
