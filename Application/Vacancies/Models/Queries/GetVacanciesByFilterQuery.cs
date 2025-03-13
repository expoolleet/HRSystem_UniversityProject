using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Models.Queries;

public class GetVacanciesByFilterQuery : IRequest<IReadOnlyCollection<Vacancy>>
{
    public Guid CompnayId { get; set; }
    public string? Title { get; set; }
}