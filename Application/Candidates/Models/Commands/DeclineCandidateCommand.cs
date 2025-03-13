using Domain.Candidates;
using MediatR;

namespace Application.Candidates.Models.Commands;

public class DeclineCandidateCommand : IRequest
{
    public required Candidate Candidate { get; set; }
}