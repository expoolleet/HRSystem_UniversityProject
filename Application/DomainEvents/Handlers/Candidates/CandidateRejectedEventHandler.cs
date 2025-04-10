using Domain.Events;
using MediatR;

namespace Application.DomainEvents.Handlers.Candidates;

public class CandidateRejectedEventHandler : INotificationHandler<DomainEventNotification<CandidateRejectedEvent>>
{
    public async Task Handle(DomainEventNotification<CandidateRejectedEvent> notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Candidate {notification.DomainEvent.CandidateId} is rejected");
    }
}