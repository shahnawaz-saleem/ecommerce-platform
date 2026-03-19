namespace Messaging;
public class ProductDeletedIntegrationEvent : IntegrationEvent
{
    public Guid ProductId { get; }

    public ProductDeletedIntegrationEvent(Guid productId)
    {
        ProductId = productId;
    }
}
