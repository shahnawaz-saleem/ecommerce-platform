using Catalog.Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Messaging;
using Catalog.Application.Interfaces;
namespace Catalog.Application.Handlers;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;
    private readonly Type _type;
    private readonly IOutboxRepository _outboxRepository;
    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger,IOutboxRepository outboxRepository)
    {
        _logger = logger;
        _type = typeof(ProductCreatedIntegrationEvent);
        _outboxRepository = outboxRepository;
    }

    public  Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product created event received. ProductId={ProductId}", notification.ProductId);

        var integrationEvent = new ProductCreatedIntegrationEvent(notification.ProductId,notification.Name,notification.Price,Guid.Empty);
         _outboxRepository.AddAsync(integrationEvent, cancellationToken);
        return  Task.CompletedTask;
    }
}
