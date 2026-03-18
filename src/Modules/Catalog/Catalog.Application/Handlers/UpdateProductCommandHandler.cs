using Catalog.Application.Interfaces;
using Catalog.Application.Products.Commands.UpdateProduct;
using Ecommerce.API.Caching;
using MediatR;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _repository;

    public UpdateProductCommandHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null)
            return false;

        product.Update(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId,
            request.StockQuantity
        );

        await _repository.SaveChangesAsync(cancellationToken);

        // cache invalidation handled by domain event handlers

        return true;
    }
}