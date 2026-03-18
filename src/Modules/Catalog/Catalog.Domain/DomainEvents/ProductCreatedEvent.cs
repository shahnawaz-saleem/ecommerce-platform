using MediatR;
namespace Catalog.Domain.DomainEvents;

public class ProductCreatedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }
    public Guid CategoryId { get; }

    public ProductCreatedEvent(Guid productId, string name, decimal price, Guid categoryId)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        CategoryId = categoryId;
    }
}
