using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly CatalogDbContext _context;

    public ProductRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<List<Product>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.CountAsync(cancellationToken);
    }
    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);

        return Task.CompletedTask;
    }

    public async Task<List<Product>> SearchAsync(string? term, Guid? categoryId, decimal? priceMin, decimal? priceMax, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(term))
        {
            var pattern = $"%{term}%";
            query = query.Where(p => EF.Functions.ILike(p.Name, pattern) || (p.Description != null && EF.Functions.ILike(p.Description, pattern)));
        }

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (priceMin.HasValue)
            query = query.Where(p => p.Price >= priceMin.Value);

        if (priceMax.HasValue)
            query = query.Where(p => p.Price <= priceMax.Value);

        query = query.OrderBy(p => p.Name).ThenBy(p => p.Id);

        return await query
            .Skip(Math.Max(0, (page - 1)) * Math.Max(1, pageSize))
            .Take(Math.Max(1, pageSize))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountSearchAsync(string? term, Guid? categoryId, decimal? priceMin, decimal? priceMax, CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(term))
        {
            var pattern = $"%{term}%";
            query = query.Where(p => EF.Functions.ILike(p.Name, pattern) || (p.Description != null && EF.Functions.ILike(p.Description, pattern)));
        }

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (priceMin.HasValue)
            query = query.Where(p => p.Price >= priceMin.Value);

        if (priceMax.HasValue)
            query = query.Where(p => p.Price <= priceMax.Value);

        return await query.CountAsync(cancellationToken);
    }

}