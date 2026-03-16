using Catalog.Application.Commands;
using Catalog.Application.Commands.UpdateProduct;
using Catalog.Application.Queries.GetProductById;
using Catalog.Application.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/catalog/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var productId = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetProduct), new { id = productId }, productId);
    }
    [HttpGet]
    public async Task<IActionResult> GetProducts(
    int page = 1,
    int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetProductsQuery(page, pageSize), cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _mediator.Send(
            new GetProductByIdQuery(id), cancellationToken);

        if (product == null)
            return NotFound();

        return Ok(product);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(
    Guid id,
    UpdateProductCommand command,
    CancellationToken cancellationToken = default)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }
}