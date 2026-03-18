using Catalog.Domain.DomainEvents;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence;

public class CatalogDbContext : DbContext
{
    private readonly IMediator _mediator;

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)");

            entity.Property(p => p.Description)
                  .HasMaxLength(1000);
            entity.HasQueryFilter(p => !p.IsDeleted);
        });
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect domain events
        var domainEntities = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IHasDomainEvents)
            .Select(e => e.Entity as IHasDomainEvents)
            .Where(e => e != null)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e!.DomainEvents)
            .ToList();

        // Save changes first
        var result = await base.SaveChangesAsync(cancellationToken);

        // Publish events
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        // Clear events
        foreach (var entity in domainEntities)
        {
            entity!.ClearDomainEvents();
        }

        return result;
    }
}
