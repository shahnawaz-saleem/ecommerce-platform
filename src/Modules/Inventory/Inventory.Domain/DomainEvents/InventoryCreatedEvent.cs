using System;

namespace Inventory.Domain.DomainEvents
{
    public class InventoryCreatedEvent : DomainEvent
    {
        public Guid ProductId { get; }
        public int TotalStock { get; }

        public InventoryCreatedEvent(Guid productId, int totalStock)
        {
            ProductId = productId;
            TotalStock = totalStock;
        }
    }
}
