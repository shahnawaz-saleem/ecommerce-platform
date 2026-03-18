using MediatR;
namespace Catalog.Domain.DomainEvents;

public class ProductDeletedEvent : DomainEvent
{
    public Guid ProductId { get; }

    public ProductDeletedEvent(Guid productId)
    {
        ProductId = productId;
    }
}
