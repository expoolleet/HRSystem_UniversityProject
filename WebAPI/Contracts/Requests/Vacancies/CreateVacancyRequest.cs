using System.ComponentModel.DataAnnotations;
using WebApi.Contracts.Dto.Vacancies;

namespace WebApi.Contracts.Requests.Vacancies;

public class CreateVacancyRequest
{
    [Required]
    public Guid CompanyId { get; init; }
    
    [Required]
    [StringLength(100)]
    public required string Description { get; init; }
    
    public required VacancyWorkflowDto Workflow { get; init; }
}