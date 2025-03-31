namespace WebApi.Contracts.Dto.Vacancies;

public class VacancyWorkflowStepDto
{
    public Guid? UserId { get; set; }
    public Guid? RoleId { get; set; }
    public required string Description { get; set; }
    public int Number { get; set; }
}