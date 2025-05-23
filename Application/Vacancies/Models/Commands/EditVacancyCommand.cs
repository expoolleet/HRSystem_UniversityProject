using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Models.Commands;

public class EditVacancyCommand : IRequest
{
    public required Vacancy Vacancy { get; init; }
    public string? Description { get; init; }
}