namespace Inventory.Domain.Entities;

public class InventoryItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public bool IsDeleted { get; set; }
}