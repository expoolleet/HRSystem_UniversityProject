using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Models.Queries;

public class GetVacanciesByFilterQuery : IRequest<IReadOnlyCollection<Vacancy>>
{
    public Guid? CompanyId { get; init; }
    public string? Title { get; init; }
}