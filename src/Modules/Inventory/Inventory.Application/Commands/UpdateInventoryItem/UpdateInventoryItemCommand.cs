using System;
using MediatR;

namespace Inventory.Application.Commands.UpdateInventoryItem;

public class UpdateInventoryItemCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public bool IsDeleted { get; set; }
}
