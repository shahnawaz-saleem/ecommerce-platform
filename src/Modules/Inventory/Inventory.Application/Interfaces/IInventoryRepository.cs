using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces;

public interface IInventoryRepository
{
    Task<InventoryItem?> GetByIdAsync(Guid id);
    Task AddAsync(InventoryItem item);
    Task UpdateAsync(InventoryItem item);
    Task<IEnumerable<InventoryItem>> GetAllAsync();
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryItem>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<InventoryItem?> GetByProductIdAsync(Guid productId);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<int> CountLowStockAsync(int threshold, CancellationToken cancellationToken = default);
    Task<int> CountOutOfStockAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryItem>> GetRecentRestocksAsync(int limit);
    Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync(int threshold, CancellationToken cancellationToken = default);
}