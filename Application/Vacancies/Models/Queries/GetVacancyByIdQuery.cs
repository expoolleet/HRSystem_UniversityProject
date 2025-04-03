using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Models.Queries;

public class GetVacancyByIdQuery : IRequest<Vacancy>
{
    public Guid VacancyId { get; init; }
}