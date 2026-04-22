using Inventory.Domain.DomainEvents;
using System;
using System.Collections.Generic;

namespace Inventory.Domain.Entities;

public class InventoryItem : Inventory.Domain.DomainEvents.IHasDomainEvents
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }

    // Total units in stock
    public int TotalStock { get; private set; }

    // Units reserved for pending orders
    public int ReservedStock { get; private set; }

    public int AvailableStock => TotalStock - ReservedStock;

    public bool IsDeleted { get; private set; }
    // Concurrency token for optimistic concurrency control
    public byte[] RowVersion { get; private set; }

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private InventoryItem() { }

    public InventoryItem(Guid productId)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        TotalStock = 0;
        ReservedStock = 0;
        IsDeleted = false;

        _domainEvents.Add(new InventoryCreatedEvent(ProductId, TotalStock));
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Invalid quantity", nameof(quantity));

        TotalStock += quantity;

        _domainEvents.Add(new InventoryUpdatedEvent(ProductId, TotalStock, ReservedStock));
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Invalid quantity", nameof(quantity));

        if (TotalStock < quantity)
            throw new InvalidOperationException("Insufficient stock");

        TotalStock -= quantity;

        _domainEvents.Add(new InventoryUpdatedEvent(ProductId, TotalStock, ReservedStock));
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Invalid quantity", nameof(quantity));

        if (AvailableStock < quantity)
            throw new InvalidOperationException("Not enough stock");

        ReservedStock += quantity;
        _domainEvents.Add(new InventoryUpdatedEvent(ProductId, TotalStock, ReservedStock));
    }

    public void ConfirmReservation(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Invalid quantity", nameof(quantity));

        if (ReservedStock < quantity)
            throw new InvalidOperationException("Not enough reserved stock to confirm");

        ReservedStock -= quantity;
        TotalStock -= quantity;

        _domainEvents.Add(new InventoryUpdatedEvent(ProductId, TotalStock, ReservedStock));
    }

    public void ReleaseReservation(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Invalid quantity", nameof(quantity));

        if (ReservedStock < quantity)
            throw new InvalidOperationException("Not enough reserved stock to release");

        ReservedStock -= quantity;
        _domainEvents.Add(new InventoryUpdatedEvent(ProductId, TotalStock, ReservedStock));
    }

    public void Delete()
    {
        IsDeleted = true;
        _domainEvents.Add(new InventoryUpdatedEvent(ProductId, TotalStock, ReservedStock));
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
