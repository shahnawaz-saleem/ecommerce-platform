using System;

namespace Catalog.Infrastructure.Persistence;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Attempts { get; set; }
}
