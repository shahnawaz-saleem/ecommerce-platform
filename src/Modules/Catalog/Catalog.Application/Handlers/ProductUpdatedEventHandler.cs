using Catalog.Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers;

public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
{
    private readonly ILogger<ProductUpdatedEventHandler> _logger;

    public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product updated event received. ProductId={ProductId}", notification.ProductId);

        // TODO: add future logic (re-indexing, projections, etc.)

        return Task.CompletedTask;
    }
}
