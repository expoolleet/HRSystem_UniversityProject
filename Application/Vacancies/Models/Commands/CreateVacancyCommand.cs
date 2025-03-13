using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Models.Commands;

public class CreateVacancyCommand : IRequest
{
    public required Vacancy Vacancy { get; set; }
}