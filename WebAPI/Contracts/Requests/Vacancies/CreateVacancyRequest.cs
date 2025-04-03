using System.ComponentModel.DataAnnotations;
using WebApi.Contracts.Dto.Vacancies;

namespace WebApi.Contracts.Requests.Vacancies;

/// <summary>
/// Request model to create a new vacancy.
/// </summary>
public class CreateVacancyRequest
{
    /// <summary>
    ///  Company ID
    /// </summary>
    [Required]
    public Guid CompanyId { get; init; }
    
    /// <summary>
    /// Vacancy description
    /// </summary>
    [Required]
    [StringLength(100)]
    public required string Description { get; init; }
    
    /// <summary>
    /// Vacancy workflow
    /// </summary>
    public required VacancyWorkflowDto Workflow { get; init; }
}