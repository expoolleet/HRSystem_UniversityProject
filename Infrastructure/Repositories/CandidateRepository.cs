using Application.Candidates.Repository;
using Domain.Candidates;
using Infrastructure.DbContext.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private CandidateDbContext _candidateDbContext;

    public CandidateRepository(CandidateDbContext candidateDbContext)
    {
        ArgumentNullException.ThrowIfNull(candidateDbContext);
        _candidateDbContext = candidateDbContext;
    }

    public async Task Add(Candidate candidate, CancellationToken cancellationToken)
    {
        await _candidateDbContext.AddAsync(candidate, cancellationToken);
    }

    public async Task<Candidate> Get(Guid id, CancellationToken cancellationToken)
    {
        var candidate = await _candidateDbContext.FindAsync<Candidate>(id, cancellationToken);
        return candidate!;
    }

    public async Task<IReadOnlyCollection<Candidate>> GetCollectionByFilter(Guid? companyId, string? title, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var query = from candidate in _candidateDbContext.Candidates
                    join vacancy in _candidateDbContext.Vacancies on candidate.VacancyId equals vacancy.Id
                    where (!companyId.HasValue || vacancy.CompanyId == companyId) &&
                          (string.IsNullOrWhiteSpace(title) || title.Contains(title))
                          select candidate;
        var candidates = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return candidates;
    }
}