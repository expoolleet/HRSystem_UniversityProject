using Domain.Vacancies;
using MediatR;

namespace Application.Companies.Models.Commands;

public class CreateVacancyCommand : IRequest
{
    public required Vacancy Vacancy { get; set; }
}