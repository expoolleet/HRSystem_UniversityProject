using Application.Candidates.Models.Queries;
using Application.Candidates.Repository;
using Domain.Candidates;
using MediatR;

namespace Application.Candidates.Handlers.QueryHandlers;

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
        var candidate = await _candidateRepository.Get(request.CandidateId, cancellationToken);

        if (candidate == null)
        {
            throw new ApplicationException("Candidate not found");
        }
        
        return candidate;
    }
}