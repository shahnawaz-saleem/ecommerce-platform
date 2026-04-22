using System;
using MediatR;

namespace Inventory.Application.Commands.CreateInventoryItem;

public class CreateInventoryItemCommand : IRequest<Guid>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
