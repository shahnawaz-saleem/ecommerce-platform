namespace Messaging;

public class ProductUpdatedIntegrationEvent : IntegrationEvent
{
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int StockQuantity { get; }

    public ProductUpdatedIntegrationEvent(Guid productId, string name, decimal price, int stockQuantity)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }
}
