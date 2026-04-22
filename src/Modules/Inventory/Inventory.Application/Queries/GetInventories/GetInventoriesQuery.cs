using System.Collections.Generic;
using MediatR;
using Inventory.Domain.Entities;
using Inventory.Application.Common;

namespace Inventory.Application.Queries.GetInventories
{
    public record GetInventoriesQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<InventoryItem>>;
}
