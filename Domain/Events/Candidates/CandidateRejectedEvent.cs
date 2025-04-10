namespace Domain.Events.Candidates;

public class CandidateRejectedEvent : IDomainEvent
{
    public Guid CandidateId { get; init; }
    public DateTime OccurredOn { get; init; }

    public CandidateRejectedEvent(Guid candidateId)
    {
        CandidateId = candidateId;
        OccurredOn = DateTime.UtcNow;
    }
}