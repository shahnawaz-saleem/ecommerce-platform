using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _db;

    public InventoryRepository(InventoryDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(InventoryItem item)
    {
        await _db.InventoryItems.AddAsync(item);
        await _db.SaveChangesAsync();
    }

    public async Task<InventoryItem?> GetByIdAsync(Guid id)
    {
        return await _db.InventoryItems.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<InventoryItem>> GetAllAsync()
    {
        return await _db.InventoryItems.ToListAsync();
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _db.InventoryItems.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryItem>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var skip = (page - 1) * pageSize;
        return await _db.InventoryItems
            .OrderBy(i => i.Id)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<InventoryItem?> GetByProductIdAsync(Guid productId)
    {
        return await _db.InventoryItems.FirstOrDefaultAsync(i => i.ProductId == productId);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(InventoryItem item)
    {
        _db.InventoryItems.Update(item);
        await _db.SaveChangesAsync();
    }
}
