using Inventory.Application.Queries.GetInventoryById;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using MediatR;
using Ecommerce.API.Caching;

namespace Inventory.Application.Handlers
{
    public class GetInventoryByIdQueryHandler : IRequestHandler<GetInventoryByIdQuery, InventoryItem?>
    {
        private readonly IInventoryRepository _repository;
        private readonly ICacheService _cache;

        public GetInventoryByIdQueryHandler(IInventoryRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<InventoryItem?> Handle(GetInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"inventory_{request.Id}";

            var cached = await _cache.GetAsync<InventoryItem>(cacheKey);

            if (cached != null)
                return cached;

            var item = await _repository.GetByIdAsync(request.Id);

            if (item != null)
                await _cache.SetAsync(cacheKey, item, TimeSpan.FromMinutes(10));

            return item;
        }
    }
}
