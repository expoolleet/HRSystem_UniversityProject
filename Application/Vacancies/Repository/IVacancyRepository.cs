using Domain.Vacancies;

namespace Application.Vacancies.Repository;

public interface IVacancyRepository
{
    Task Create(
        Vacancy vacancy, 
        CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<Vacancy>> GetCollectionByFilter(
        Guid? companyId,
        string? title, 
        CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<Vacancy>> GetShortCollectionByFilter(
        Guid? companyId,
        string? title, 
        CancellationToken cancellationToken);

    Task<Vacancy> Get(
        Guid vacancyId,
        CancellationToken cancellationToken);

    Task<Vacancy> GetShort(
        Guid vacancyId,
        CancellationToken cancellationToken);

    Task Edit(
        Vacancy vacancy,
        string? description,
        CancellationToken cancellationToken);
}