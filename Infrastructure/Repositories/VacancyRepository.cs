using Application.Vacancies.Repository;
using Domain.Vacancies;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VacancyRepository : IVacancyRepository
{
    private readonly MainDbContext _dbContext;

    public VacancyRepository(MainDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task Create(Vacancy vacancy, CancellationToken cancellationToken)
    {
        await _dbContext.Vacancies.AddAsync(vacancy, cancellationToken);
    }
    
    public async Task<Vacancy> Get(Guid vacancyId, CancellationToken cancellationToken)
    {
        var vacancy = await _dbContext.Vacancies
            .Include(v => v.Workflow)
            .FirstAsync(v => v.Id == vacancyId, cancellationToken);
        return vacancy;
    }

    public async Task<Vacancy> GetShort(Guid vacancyId, CancellationToken cancellationToken)
    {
        var vacancy = await _dbContext.Vacancies
            .FindAsync(vacancyId, cancellationToken);
        return vacancy!;
    }

    public async Task Edit(Vacancy vacancy, string? description, CancellationToken cancellationToken)
    {
        var trackedVacancy = await _dbContext.Vacancies.FindAsync(vacancy.Id, cancellationToken);

        if (trackedVacancy != null)
        {
            trackedVacancy.Description = description;
        }
        else
        {
            vacancy.Description = description;
            _dbContext.Attach(vacancy);
            _dbContext.Entry(vacancy).State = EntityState.Modified;
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
        
    public async Task<IReadOnlyCollection<Vacancy>> GetCollectionByFilter(Guid? companyId, string? title, CancellationToken cancellationToken)
    {
        IQueryable<Vacancy> query = _dbContext.Vacancies;
        if (companyId.HasValue)
        {
            query = query.Where(v => v.CompanyId == companyId);
        }
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(v => v.Description != null && v.Description.Contains(title));
        }
        query.Include(v => v.Workflow);
        return await query.ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyCollection<Vacancy>> GetShortCollectionByFilter(Guid? companyId, string? title, CancellationToken cancellationToken)
    {
        IQueryable<Vacancy> query = _dbContext.Vacancies;
        if (companyId.HasValue)
        {
            query = query.Where(v => v.CompanyId == companyId);
        }
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(v => v.Description != null && v.Description.Contains(title));
        }
        return await query.ToListAsync(cancellationToken);
    }
}