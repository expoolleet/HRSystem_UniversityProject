using Domain.Candidates;
using MediatR;

namespace Application.Companies.Models.Commands;

public class AcceptCandidateCommand : IRequest
{
    public required Candidate Candidate { get; set; }
}