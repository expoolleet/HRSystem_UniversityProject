using Domain.Candidates;

namespace Application.Candidates.Repositories;

public interface ICandidateRepository
{ 
    Task Create(
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
    Task SaveChangesAsync();
}