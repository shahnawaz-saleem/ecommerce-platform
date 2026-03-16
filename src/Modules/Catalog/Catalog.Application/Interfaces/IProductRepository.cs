using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces;

public interface IProductRepository
{
    Task AddAsync(Product product);

    Task<Product?> GetByIdAsync(Guid id);

    Task SaveChangesAsync();
    Task<List<Product>> GetAllAsync();
    public  Task<List<Product>> GetPagedAsync(int page, int pageSize);
    public Task<int> CountAsync();
    
    }