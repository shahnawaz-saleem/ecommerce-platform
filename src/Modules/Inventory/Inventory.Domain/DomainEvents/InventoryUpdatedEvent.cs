using System;

namespace Inventory.Domain.DomainEvents
{
    public class InventoryUpdatedEvent : DomainEvent
    {
        public Guid ProductId { get; }
        public int TotalStock { get; }
        public int ReservedStock { get; }

        public InventoryUpdatedEvent(Guid productId, int totalStock, int reservedStock)
        {
            ProductId = productId;
            TotalStock = totalStock;
            ReservedStock = reservedStock;
        }
    }
}
