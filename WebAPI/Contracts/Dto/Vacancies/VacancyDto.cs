namespace WebApi.Contracts.Dto.Vacancies;

public class VacancyDto
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; init; }
    public required string Description { get;  init; }
    public required VacancyWorkflowDto Workflow { get; init; }
}