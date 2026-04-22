using Inventory.Domain.DomainEvents;
using Ecommerce.API.Caching;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Application.Handlers
{
    public class InventoryCacheInvalidationHandler :
        INotificationHandler<InventoryCreatedEvent>,
        INotificationHandler<InventoryUpdatedEvent>
    {
        private readonly ICacheService _cache;

        public InventoryCacheInvalidationHandler(ICacheService cache)
        {
            _cache = cache;
        }

        public async Task Handle(InventoryCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _cache.RemoveAsync("inventories");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cache invalidation failed on InventoryCreatedEvent: {ex.Message}");
            }
        }

        public async Task Handle(InventoryUpdatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _cache.RemoveAsync($"inventory_{notification.ProductId}");
                await _cache.RemoveAsync("inventories");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cache invalidation failed on InventoryUpdatedEvent: {ex.Message}");
            }
        }
    }
}
