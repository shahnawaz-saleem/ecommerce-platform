using MediatR;
using Catalog.Domain.Entities;

namespace Catalog.Application.Queries.GetProducts;

public record GetProductsQuery() : IRequest<List<Product>>;