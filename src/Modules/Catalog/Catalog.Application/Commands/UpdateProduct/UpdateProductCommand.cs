using System;
using MediatR;

namespace Catalog.Application.Commands.UpdateProduct
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int StockQuantity,
        Guid CategoryId,
        Guid vendorID
        ) : IRequest<bool>
    {
    }
}
