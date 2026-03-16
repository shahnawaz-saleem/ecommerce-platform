using Catalog.Application.Commands.UpdateProduct;
using Catalog.Application.Interfaces;
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
        var product = await _repository.GetByIdAsync(request.Id);

        if (product == null)
            throw new Exception("Product not found");

        product.Update(
            request.Name,
            request.Description,
            request.Price,
            request.CategoryId,
            request.StockQuantity
        );

        await _repository.SaveChangesAsync();

        // invalidate cache
        await _cache.RemoveAsync($"product_{request.Id}");
        await _cache.RemoveAsync("products");

        return true;
    }
}