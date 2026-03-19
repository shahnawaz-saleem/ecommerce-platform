namespace Messaging;

public class ProductCreatedIntegrationEvent : IntegrationEvent
{
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }
    public Guid CategoryId { get; }

    public ProductCreatedIntegrationEvent(Guid productId, string name, decimal price, Guid categoryId)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        CategoryId = categoryId;
    }
}
