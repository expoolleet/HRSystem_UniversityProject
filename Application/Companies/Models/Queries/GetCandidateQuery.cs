using Domain.Candidates;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Models.Queries;

public class GetCandidateQuery : IRequest<Candidate>
{
    public Guid CandidateId { get; set; }
}