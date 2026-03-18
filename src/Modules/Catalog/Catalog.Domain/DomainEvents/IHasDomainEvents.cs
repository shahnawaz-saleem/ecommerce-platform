using MediatR;
using System.Collections.ObjectModel;

namespace Catalog.Domain.DomainEvents;

public interface IHasDomainEvents
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
