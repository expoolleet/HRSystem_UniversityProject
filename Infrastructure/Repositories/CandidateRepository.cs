using Application.Candidates.Repositories;
using Domain.Candidates;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly MainDbContext _dbContext;

    public CandidateRepository(MainDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task Create(Candidate candidate, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(candidate, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Candidate> Get(Guid id, CancellationToken cancellationToken)
    {
        var candidate = await _dbContext.Candidates.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return candidate!;
    }

    public async Task<IReadOnlyCollection<Candidate>> GetCollectionByFilter(Guid? companyId, string? title, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Candidates
            .Join(_dbContext.Vacancies,
                candidate => candidate.VacancyId,
                vacancy => vacancy.Id,
                (candidate, vacancy) => new { Candidate = candidate, Vacancy = vacancy })
            .Where(cv => (!companyId.HasValue || cv.Vacancy.CompanyId == companyId) &&
                         cv.Vacancy.Description != null &&
                         (string.IsNullOrEmpty(title) || cv.Vacancy.Description.Contains(title)))
            .Select(cv => cv.Candidate);
        
        var candidates = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return candidates;
    }
}