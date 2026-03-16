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
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);

        return Ok(productId);
    }
    [HttpGet]
    public async Task<IActionResult> GetProducts(
    int page = 1,
    int pageSize = 10)
    {
        var result = await _mediator.Send(
            new GetProductsQuery(page, pageSize));

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _mediator.Send(
            new GetProductByIdQuery(id));

        return Ok(product);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(
    Guid id,
    UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}