using Domain.Events;
using MediatR;

namespace Application.DomainEvents;

public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        DomainEvent = domainEvent;
    }
}