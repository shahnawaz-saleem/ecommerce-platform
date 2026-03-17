using Catalog.Application.Common;
using MediatR;
using Catalog.Domain.Entities;

namespace Catalog.Application.Queries.SearchProducts;

public record SearchProductsQuery(
    string? Term = null,
    Guid? CategoryId = null,
    decimal? PriceMin = null,
    decimal? PriceMax = null,
    int Page = 1,
    int PageSize = 10
) : IRequest<PagedResult<Product>>;
