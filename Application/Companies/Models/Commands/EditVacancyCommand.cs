using Domain.Vacancies;
using MediatR;

namespace Application.Companies.Models.Commands;

public class EditVacancyCommand : IRequest
{
    public required Vacancy Vacancy { get; set; }
    public string? Description { get; set; }
}