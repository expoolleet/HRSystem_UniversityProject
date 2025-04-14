using MediatR;

namespace Application.Candidates.Models.Commands;

public class RejectCandidateCommand : IRequest
{
    public Guid CandidateId { get; init; }
    public Guid UserId { get; init; }
    public required string Feedback { get; init; }
}