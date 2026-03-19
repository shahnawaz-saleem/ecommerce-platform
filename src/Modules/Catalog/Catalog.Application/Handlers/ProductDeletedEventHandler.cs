using Catalog.Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Messaging;
using Catalog.Application.Interfaces;

namespace Catalog.Application.Handlers;

public class ProductDeletedEventHandler : INotificationHandler<ProductDeletedEvent>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger;
    private readonly IOutboxRepository _outboxRepository;

    public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger, IOutboxRepository outboxRepository)
    {
        _logger = logger;
        _outboxRepository = outboxRepository;
    }

    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product deleted event received. ProductId={ProductId}", notification.ProductId);

        var integrationEvent = new ProductDeletedIntegrationEvent(notification.ProductId);

        await _outboxRepository.AddAsync(integrationEvent, cancellationToken);
    }
}
