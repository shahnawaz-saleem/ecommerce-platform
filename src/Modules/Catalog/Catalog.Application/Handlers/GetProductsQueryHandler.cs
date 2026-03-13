using MediatR;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Ecommerce.API.Caching;

namespace Catalog.Application.Queries.GetProducts;

public class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, List<Product>>
{
    private readonly IProductRepository _repository;
    private readonly ICacheService _cache;
    public GetProductsQueryHandler(IProductRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<List<Product>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        const string cacheKey = "products";

        var cached = await _cache.GetAsync<List<Product>>(cacheKey);

        if (cached != null)
            return cached;

        var products = await _repository.GetAllAsync();

        await _cache.SetAsync(
            cacheKey,
            products,
            TimeSpan.FromMinutes(5));

        return products;
    }
}