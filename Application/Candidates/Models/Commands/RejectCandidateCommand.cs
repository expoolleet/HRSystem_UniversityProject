using Domain.Candidates;
using MediatR;

namespace Application.Candidates.Models.Commands;

public class RejectCandidateCommand : IRequest
{
    public Guid CandidateId { get; set; }
    public Guid UserId { get; set; }
    public string Feedback { get; set; } = string.Empty;
}