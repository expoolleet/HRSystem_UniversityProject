using Domain.Candidates;
using MediatR;

namespace Application.Candidates.Models.Queries;

public class GetCandidatesByFilterQuery : IRequest<IReadOnlyCollection<Candidate>>
{
    public Guid? CompanyId { get; init; }
    public string? Title { get; init; }
    
    public int Page { get; init; }
    
    public int PageSize { get; init; }
}