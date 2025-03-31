using Domain.Vacancies;
using MediatR;

namespace Application.Vacancies.Models.Commands;

public class CreateVacancyCommand : IRequest<Guid>
{
    public Guid CompanyId { get; init; }
    public string? Description { get; init; }
    public VacancyWorkflow? Workflow { get; init; }
}