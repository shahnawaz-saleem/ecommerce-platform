using System.Collections.Generic;

namespace Inventory.Domain.DomainEvents
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
