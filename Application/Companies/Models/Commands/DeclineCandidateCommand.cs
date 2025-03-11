using Domain.Candidates;
using MediatR;

namespace Application.Companies.Models.Commands;

public class DeclineCandidateCommand : IRequest
{
    public required Candidate Candidate { get; set; }
}