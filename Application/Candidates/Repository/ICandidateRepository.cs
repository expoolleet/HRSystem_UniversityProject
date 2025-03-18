using Domain.Candidates;

namespace Application.Candidates.Repository;

public interface ICandidateRepository
{ 
    Task Add(
        Candidate candidate,
        CancellationToken cancellationToken);
    
    Task<Candidate> Get(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Candidate>> GetCollectionByFilter(
        Guid? companyId,
        string? title,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}