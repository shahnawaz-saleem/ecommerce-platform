using Catalog.Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Messaging;
using Catalog.Application.Interfaces;

namespace Catalog.Application.Handlers;

public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
{
    private readonly ILogger<ProductUpdatedEventHandler> _logger;
    private readonly IOutboxRepository _outboxRepository;

    public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger, IOutboxRepository outboxRepository)
    {
        _logger = logger;
        _outboxRepository = outboxRepository;
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product updated event received. ProductId={ProductId}", notification.ProductId);

        var integrationEvent = new ProductUpdatedIntegrationEvent(notification.ProductId, notification.Name, notification.Price, notification.StockQuantity);

        await _outboxRepository.AddAsync(integrationEvent, cancellationToken);
    }
}
