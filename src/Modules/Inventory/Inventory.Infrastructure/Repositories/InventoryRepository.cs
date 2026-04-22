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

    public async Task UpdateAsync(InventoryItem item)
    {
        _db.InventoryItems.Update(item);
        await _db.SaveChangesAsync();
    }
}
