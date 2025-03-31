using Domain.Candidates;
using MediatR;

namespace Application.Candidates.Models.Queries;

public class GetCandidateQuery : IRequest<Candidate>
{
    public Guid CandidateId { get; init; }
}