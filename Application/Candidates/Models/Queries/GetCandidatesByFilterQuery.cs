using Domain.Candidates;
using MediatR;

namespace Application.Candidates.Models.Queries;

public class GetCandidatesByFilterQuery : IRequest<IReadOnlyCollection<Candidate>>
{
    public Guid? CompanyId { get; set; }
    public string? Title { get; set; }
    
    public int Pages { get; set; }
    
    public int PageSize { get; set; }
}