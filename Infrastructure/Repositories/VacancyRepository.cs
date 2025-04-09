using Application.Vacancies.Repositories;
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
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Vacancy> Get(Guid id, CancellationToken cancellationToken)
    {
        var vacancy = await _dbContext.Vacancies
            .FirstAsync(v => v.Id == id, cancellationToken);
        return vacancy;
    }

    public async Task Edit(Vacancy vacancy, string? description, CancellationToken cancellationToken)
    {
        var trackedVacancy = await _dbContext.Vacancies.FindAsync(vacancy.Id, cancellationToken);

        if (trackedVacancy != null)
        {
            trackedVacancy.Edit(description);
        }
        else
        {
            vacancy.Edit(description);
            _dbContext.Attach(vacancy);
            _dbContext.Entry(vacancy).State = EntityState.Modified;
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
        
    public async Task<IReadOnlyCollection<Vacancy>> GetCollectionByFilter(Guid? companyId, string? title, CancellationToken cancellationToken)
    {
        IQueryable<Vacancy> query = _dbContext.Vacancies
            .AsNoTracking();
        if (companyId.HasValue)
        {
            query = query.Where(v => v.CompanyId == companyId);
        }
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(v => v.Description != null && v.Description.ToLower().Contains(title.ToLower()));
        }
        return await query.ToListAsync(cancellationToken);
    }
}