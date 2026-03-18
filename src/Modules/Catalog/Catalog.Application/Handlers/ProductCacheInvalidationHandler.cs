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
        try
        {
            // Invalidate list caches when a new product is created
            await _cache.RemoveAsync("products");
        }
        catch (Exception ex)
        {
            // Log and swallow cache errors to avoid affecting domain flow
            Console.WriteLine($"Cache invalidation failed on ProductCreatedEvent: {ex.Message}");
        }
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _cache.RemoveAsync($"product_{notification.ProductId}");
            await _cache.RemoveAsync("products");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cache invalidation failed on ProductUpdatedEvent: {ex.Message}");
        }
    }

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _cache.RemoveAsync($"product_{notification.ProductId}");
            await _cache.RemoveAsync("products");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cache invalidation failed on ProductDeletedEvent: {ex.Message}");
        }
    }
}
