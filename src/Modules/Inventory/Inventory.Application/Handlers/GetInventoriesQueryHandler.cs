using Inventory.Application.Queries.GetInventories;
using Inventory.Application.Interfaces;
using Inventory.Application.Common;
using Inventory.Domain.Entities;
using MediatR;
using Ecommerce.API.Caching;

namespace Inventory.Application.Handlers
{
    public class GetInventoriesQueryHandler : IRequestHandler<GetInventoriesQuery, PagedResult<InventoryItem>>
    {
        private readonly IInventoryRepository _repository;
        private readonly ICacheService _cache;

        public GetInventoriesQueryHandler(IInventoryRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<PagedResult<InventoryItem>> Handle(GetInventoriesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"inventories_page_{request.Page}_{request.PageSize}";

            var cached = await _cache.GetAsync<PagedResult<InventoryItem>>(cacheKey);

            if (cached != null)
                return cached;

            var totalCount = await _repository.CountAsync(cancellationToken);

            var items = await _repository.GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            var result = new PagedResult<InventoryItem>
            {
                Items = items,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }
    }
}
