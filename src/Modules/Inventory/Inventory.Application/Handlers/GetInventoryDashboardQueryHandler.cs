using Inventory.Application.Queries.GetInventoryDashboard;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using MediatR;
using Ecommerce.API.Caching;

namespace Inventory.Application.Handlers
{
    public class GetInventoryDashboardQueryHandler : IRequestHandler<GetInventoryDashboardQuery, DashboardDto>
    {
        private readonly IInventoryRepository _repository;
        private readonly ICacheService _cache;

        public GetInventoryDashboardQueryHandler(IInventoryRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<DashboardDto> Handle(GetInventoryDashboardQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "inventory_dashboard";

            var cached = await _cache.GetAsync<DashboardDto>(cacheKey);
            if (cached != null)
                return cached;

            var total = await _repository.CountAsync(cancellationToken);
            var lowStock = await _repository.CountLowStockAsync(5, cancellationToken);
            var outOfStock = await _repository.CountOutOfStockAsync(cancellationToken);
            var recent = await _repository.GetRecentRestocksAsync(5);

            var dto = new DashboardDto
            {
                TotalProducts = total,
                LowStockItems = lowStock,
                OutOfStockItems = outOfStock,
                RecentRestocks = recent.Select(i => new RecentRestockDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.TotalStock,
                    LastRestockedAt = i.LastRestockedAt
                }).ToList()
            };

            await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(1));

            return dto;
        }
    }
}
