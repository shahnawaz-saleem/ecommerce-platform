using Catalog.Application.Common;
using Catalog.Application.Interfaces;
using Catalog.Application.Queries.SearchProducts;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Handlers;

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, PagedResult<Product>>
{
    private readonly IProductRepository _repository;

    public SearchProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<Product>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var total = await _repository.CountSearchAsync(request.Term, request.CategoryId, request.PriceMin, request.PriceMax, cancellationToken);

        var items = await _repository.SearchAsync(request.Term, request.CategoryId, request.PriceMin, request.PriceMax, request.Page, request.PageSize, cancellationToken);

        return new PagedResult<Product>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = total
        };
    }
}
