using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Application.Products.Commands.DeleteProduct;
using Catalog.Application.Products.Commands.UpdateProduct;
using Catalog.Application.Queries.GetProductById;
using Catalog.Application.Queries.GetProducts;
using Catalog.Application.Queries.SearchProducts;
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
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteProductCommand(id);

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts(
        [FromQuery] string? term,
        [FromQuery] Guid? categoryId,
        [FromQuery] decimal? priceMin,
        [FromQuery] decimal? priceMax,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (page < 1)
            return BadRequest("Page must be >= 1");

        const int maxPageSize = 100;
        pageSize = Math.Clamp(pageSize, 1, maxPageSize);

        var query = new SearchProductsQuery(term, categoryId, priceMin, priceMax, page, pageSize);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}