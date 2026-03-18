using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Catalog.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Outbox;

public class OutboxDispatcher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxDispatcher> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    public OutboxDispatcher(IServiceScopeFactory scopeFactory, ILogger<OutboxDispatcher> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox dispatcher started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var pending = await db.OutboxMessages
                    .Where(x => x.ProcessedAt == null)
                    .OrderBy(x => x.OccurredOn)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                foreach (var msg in pending)
                {
                    try
                    {
                        var type = Type.GetType(msg.Type);
                        if (type == null)
                        {
                            _logger.LogWarning("Outbox message type not found: {Type}", msg.Type);
                            msg.Attempts++;
                            msg.ProcessedAt = DateTime.UtcNow;
                            await db.SaveChangesAsync(stoppingToken);
                            continue;
                        }

                        var evt = JsonSerializer.Deserialize(msg.Content, type);
                        if (evt is MediatR.INotification notification)
                        {
                            await mediator.Publish(notification, stoppingToken);
                        }

                        msg.ProcessedAt = DateTime.UtcNow;
                        msg.Attempts++;
                        await db.SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process outbox message {Id}", msg.Id);
                        msg.Attempts++;
                        await db.SaveChangesAsync(stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox dispatcher encountered an error");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Outbox dispatcher stopping");
    }
}
