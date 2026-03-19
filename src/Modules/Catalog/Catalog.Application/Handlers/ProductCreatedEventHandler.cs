using Catalog.Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Messaging;
using Catalog.Application.Interfaces;
namespace Catalog.Application.Handlers;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;
    private readonly IOutboxRepository _outboxRepository;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger, IOutboxRepository outboxRepository)
    {
        _logger = logger;
        _outboxRepository = outboxRepository;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product created event received. ProductId={ProductId}", notification.ProductId);

        var integrationEvent = new ProductCreatedIntegrationEvent(notification.ProductId, notification.Name, notification.Price, notification.CategoryId);

        // Persist integration event to outbox so it will be dispatched reliably
        await _outboxRepository.AddAsync(integrationEvent, cancellationToken);
    }
}
