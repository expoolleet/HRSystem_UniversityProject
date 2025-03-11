using Application.Companies.Models.Queries;
using Application.Companies.RepositoryInterfaces;
using Application.Companies.Services;
using Domain.Candidates;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.QueryHandlers;

public class GetCandidateQueryHandler : IRequestHandler<GetCandidateQuery, Candidate>
{
    private readonly ICandidateRepository _candidateRepository;
    
    public GetCandidateQueryHandler(ICandidateRepository candidateRepository)
    {
        ArgumentNullException.ThrowIfNull(candidateRepository);
        _candidateRepository = candidateRepository;
    }
    
    public async Task<Candidate> Handle(GetCandidateQuery request, CancellationToken cancellationToken)
    {
        return await _candidateRepository.GetCandidate(request.CandidateId, cancellationToken);
    }
}