using System;
using MediatR;
using Catalog.Domain.Entities;

namespace Catalog.Application.Queries.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Product>;
    
  
}
