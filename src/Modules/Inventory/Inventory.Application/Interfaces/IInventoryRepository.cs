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
}