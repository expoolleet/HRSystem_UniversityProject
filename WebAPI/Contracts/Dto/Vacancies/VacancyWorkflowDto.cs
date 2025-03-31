namespace WebApi.Contracts.Dto.Vacancies;

public class VacancyWorkflowDto
{
    public required IReadOnlyCollection<VacancyWorkflowStepDto> Steps { get; set; }
}