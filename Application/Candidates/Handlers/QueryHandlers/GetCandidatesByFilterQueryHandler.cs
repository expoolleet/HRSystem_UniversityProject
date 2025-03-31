using Application.Candidates.Models.Queries;
using Application.Candidates.Repository;
using Domain.Candidates;
using MediatR;

namespace Application.Candidates.Handlers.QueryHandlers;

public class GetCandidatesByFilterQueryHandler : IRequestHandler<GetCandidatesByFilterQuery, IReadOnlyCollection<Candidate>>
{
    private readonly ICandidateRepository _candidateRepository;
    
    public GetCandidatesByFilterQueryHandler(ICandidateRepository candidateRepository)
    {
        ArgumentNullException.ThrowIfNull(candidateRepository);
        _candidateRepository = candidateRepository;
    }
    
    public async Task<IReadOnlyCollection<Candidate>> Handle(GetCandidatesByFilterQuery request, CancellationToken cancellationToken)
    {
        return await _candidateRepository.GetCollectionByFilter(
            request.CompanyId,
            request.Title,
            request.Page,
            request.PageSize,
            cancellationToken);
    }
}