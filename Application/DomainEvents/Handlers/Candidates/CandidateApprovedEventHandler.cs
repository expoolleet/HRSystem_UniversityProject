using Domain.Events;
using Domain.Events.Candidates;
using MediatR;

namespace Application.DomainEvents.Handlers.Candidates;

public class CandidateApprovedEventHandler : INotificationHandler<DomainEventNotification<CandidateApprovedEvent>>
{
    public async Task Handle(DomainEventNotification<CandidateApprovedEvent> notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Candidate {notification.DomainEvent.CandidateId} is approved");
    }
}