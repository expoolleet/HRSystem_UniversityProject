using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Models.Commands;

public class AddVacancyCommand : IRequest
{
    public required Vacancy Vacancy { get; init; }
}