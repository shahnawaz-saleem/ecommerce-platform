using Catalog.Application.Common;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Ecommerce.API.Caching;
using MediatR;

namespace Catalog.Application.Queries.GetProducts;

public class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, PagedResult<Product>>
{
    private readonly IProductRepository _repository;
    private readonly ICacheService _cache;
    public GetProductsQueryHandler(IProductRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<PagedResult<Product>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"products_page_{request.Page}_{request.PageSize}";

        var cached = await _cache.GetAsync<PagedResult<Product>>(cacheKey);

        if (cached != null)
            return cached;

        var totalCount = await _repository.CountAsync(cancellationToken);

        var products = await _repository.GetPagedAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        var result = new PagedResult<Product>
        {
            Items = products,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
        return result;
    }
}