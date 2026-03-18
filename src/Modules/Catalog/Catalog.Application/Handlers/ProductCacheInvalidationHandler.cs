using Catalog.Domain.DomainEvents;
using Ecommerce.API.Caching;
using MediatR;

namespace Catalog.Application.Handlers;

public class ProductCacheInvalidationHandler :
    INotificationHandler<ProductCreatedEvent>,
    INotificationHandler<ProductUpdatedEvent>,
    INotificationHandler<ProductDeletedEvent>
{
    private readonly ICacheService _cache;

    public ProductCacheInvalidationHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Invalidate list caches when a new product is created
        await _cache.RemoveAsync("products");
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"product_{notification.ProductId}");
        await _cache.RemoveAsync("products");
    }

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"product_{notification.ProductId}");
        await _cache.RemoveAsync("products");
    }
}
