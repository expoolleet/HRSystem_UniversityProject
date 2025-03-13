using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Models.Queries;

public class GetVacancyQuery : IRequest<Vacancy>
{
    public Guid VacancyId { get; set; }
}