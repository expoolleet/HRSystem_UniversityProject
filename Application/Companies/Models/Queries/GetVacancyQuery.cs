using Domain.Candidates;
using Domain.Companies;
using Domain.Vacancies;
using MediatR;

namespace Application.Companies.Models.Queries;

public class GetVacancyQuery : IRequest<Vacancy>
{
    public Guid VacancyId { get; set; }
}