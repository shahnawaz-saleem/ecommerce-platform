using Inventory.Application.Queries.GetLowStockAlerts;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using MediatR;
using Ecommerce.API.Caching;

namespace Inventory.Application.Handlers
{
    public class GetLowStockAlertsQueryHandler : IRequestHandler<GetLowStockAlertsQuery, List<LowStockAlertDto>>
    {
        private readonly IInventoryRepository _repository;
        private readonly ICacheService _cache;

        public GetLowStockAlertsQueryHandler(IInventoryRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<LowStockAlertDto>> Handle(GetLowStockAlertsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"inventory_lowstock_{request.Threshold}";

            var cached = await _cache.GetAsync<List<LowStockAlertDto>>(cacheKey);
            if (cached != null)
                return cached;

            var items = await _repository.GetLowStockItemsAsync(request.Threshold, cancellationToken);

            var list = items.Select(i => new LowStockAlertDto
            {
                ProductId = i.ProductId,
                AvailableStock = i.TotalStock - i.ReservedStock,
                Message = i.TotalStock - i.ReservedStock <= 0 ? "Out of stock" : "Low stock"
            }).ToList();

            await _cache.SetAsync(cacheKey, list, TimeSpan.FromMinutes(1));

            return list;
        }
    }
}
