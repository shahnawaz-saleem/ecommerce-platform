using MediatR;

namespace Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Guid>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public Guid CategoryId { get; set; }

    public Guid VendorId { get; set; }

    public int StockQuantity { get; set; }
}