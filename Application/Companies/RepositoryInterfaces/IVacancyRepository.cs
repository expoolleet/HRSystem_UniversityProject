using Domain.Vacancies;

namespace Application.Companies.RepositoryInterfaces;

public interface IVacancyRepository
{
    Task Create(
        Vacancy vacancy, 
        CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<Vacancy>> GetVacanciesByFilter(
        Guid? userId,
        Guid? companyId,
        string? title, 
        CancellationToken cancellationToken);

    Task<Vacancy> GetVacancy(
        Guid? userId,
        Guid vacancyId,
        CancellationToken cancellationToken);

    Task Edit(
        Vacancy vacancy,
        string? description,
        CancellationToken cancellationToken);
    
    Task Reply(
        Guid vacancyId, 
        CancellationToken cancellationToken);
}