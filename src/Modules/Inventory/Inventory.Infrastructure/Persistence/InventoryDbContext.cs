using Inventory.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence;

public class InventoryDbContext : DbContext
{
    private readonly IMediator _mediator;

    public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<InventoryItem> InventoryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.TotalStock);
            entity.Property(p => p.ReservedStock);
            entity.Ignore(i => i.DomainEvents);
            // Configure RowVersion as concurrency token
            entity.Property<byte[]>("RowVersion").IsRowVersion();
            entity.Property(p => p.CreatedAt);
            entity.Property(p => p.LastRestockedAt);
            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect domain events
        var domainEntities = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Inventory.Domain.DomainEvents.IHasDomainEvents)
            .Select(e => e.Entity as Inventory.Domain.DomainEvents.IHasDomainEvents)
            .Where(e => e != null)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e!.DomainEvents)
            .ToList();

        // Publish events in-process
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        // Clear events
        foreach (var entity in domainEntities)
        {
            entity!.ClearDomainEvents();
        }

        // Save changes (including outbox) within the same transaction
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
