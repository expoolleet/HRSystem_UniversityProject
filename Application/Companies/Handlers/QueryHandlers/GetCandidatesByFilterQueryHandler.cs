using Application.Companies.Models.Queries;
using Application.Companies.RepositoryInterfaces;
using Application.Companies.Services;
using Domain.Candidates;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.QueryHandlers;

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
        return await _candidateRepository.GetCandidatesByFilter(
            request.CompanyId,
            request.Title,
            request.Pages,
            request.PageSize,
            cancellationToken);
    }
}