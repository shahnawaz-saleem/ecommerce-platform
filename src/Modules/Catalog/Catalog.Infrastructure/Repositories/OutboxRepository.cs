using Catalog.Application.Interfaces;
using Catalog.Domain.DomainEvents;
using Catalog.Infrastructure.Persistence;
using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly CatalogDbContext _context;

        public OutboxRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOn = DateTime.UtcNow,
                Type = integrationEvent.GetType().AssemblyQualifiedName ?? integrationEvent.GetType().FullName,
                Content = System.Text.Json.JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()),
                Attempts = 0
            };

            await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        }
    }
}
