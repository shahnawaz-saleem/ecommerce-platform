using MediatR;
using System;

namespace Inventory.Domain.DomainEvents
{
    public class DomainEvent : INotification
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
