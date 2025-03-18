using Domain.Vacancies;

namespace Application.Vacancies.Repository;

public interface IVacancyRepository
{
    Task Add(
        Vacancy vacancy, 
        CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<Vacancy>> GetCollectionByFilter(
        Guid? userId,
        Guid? companyId,
        string? title, 
        CancellationToken cancellationToken);

    Task<Vacancy> Get(
        Guid? userId,
        Guid vacancyId,
        CancellationToken cancellationToken);

    Task Edit(
        Vacancy vacancy,
        string description,
        CancellationToken cancellationToken);
}