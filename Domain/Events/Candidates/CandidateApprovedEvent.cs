namespace Domain.Events;

public class CandidateApprovedEvent : IDomainEvent
{
    public Guid CandidateId { get; init; }
    public DateTime OccurredOn { get; init; }

    public CandidateApprovedEvent(Guid candidateId)
    {
        CandidateId = candidateId;
        OccurredOn = DateTime.UtcNow;
    }
}