using Catalog.Application.Interfaces;
using Catalog.Application.Products.Commands.UpdateProduct;
using Ecommerce.API.Caching;
using MediatR;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _repository;
    private readonly ICacheService _cache;

    public UpdateProductCommandHandler(
        IProductRepository repository,
        ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
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

        // invalidate cache
        await _cache.RemoveAsync($"product_{request.Id}");
        await _cache.RemoveAsync("products");

        return true;
    }
}