using System;
using MediatR;
using Inventory.Domain.Entities;

namespace Inventory.Application.Queries.GetInventoryById
{
    public record GetInventoryByIdQuery(Guid Id) : IRequest<InventoryItem?>;
}
