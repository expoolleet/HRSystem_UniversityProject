using Domain.Candidates;
using MediatR;

namespace Application.Candidates.Models.Commands;

public class AcceptCandidateCommand : IRequest
{
    public required Candidate Candidate { get; set; }
}