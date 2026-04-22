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
            entity.Property(p => p.Quantity);
            entity.Ignore("DomainEvents");
            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Publish domain events if any (no-op by default)
        return await base.SaveChangesAsync(cancellationToken);
    }
}
