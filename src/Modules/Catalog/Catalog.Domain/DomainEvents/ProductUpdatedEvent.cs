using MediatR;
namespace Catalog.Domain.DomainEvents;

public class ProductUpdatedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int StockQuantity { get; }

    public ProductUpdatedEvent(Guid productId, string name, decimal price, int stockQuantity)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }
}
