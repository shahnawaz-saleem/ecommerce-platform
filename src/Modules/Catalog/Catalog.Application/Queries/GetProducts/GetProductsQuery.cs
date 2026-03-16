using MediatR;
using Catalog.Domain.Entities;
using Catalog.Application.Common;
namespace Catalog.Application.Queries.GetProducts;

public record GetProductsQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<Product>>;