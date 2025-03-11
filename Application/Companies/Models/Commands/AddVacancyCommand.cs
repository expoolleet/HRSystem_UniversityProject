using Domain.Vacancies;
using MediatR;

namespace Application.Companies.Models.Commands;

public class AddVacancyCommand : IRequest
{
    public required Vacancy Vacancy { get; set; }
}