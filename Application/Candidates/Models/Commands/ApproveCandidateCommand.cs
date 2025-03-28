using Domain.Candidates;
using Domain.Companies;
using MediatR;

namespace Application.Candidates.Models.Commands;

public class ApproveCandidateCommand : IRequest
{
    public Guid CandidateId { get; set; }
    public Guid UserId { get; set; }
    public required string Feedback { get; set; }
}