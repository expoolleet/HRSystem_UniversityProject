using Domain.Candidates;

namespace Application.Candidates.Repository;

public interface ICandidateRepository
{ 
    Task Accept(
        Candidate candidate,
        CancellationToken cancellationToken);

    Task Decline(
        Candidate candidate, 
        CancellationToken cancellationToken);

    Task<Candidate> GetCandidate(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Candidate>> GetCandidatesByFilter(
        Guid? companyId,
        string? title,
        int pages,
        int pageSize,
        CancellationToken cancellationToken
        );
}