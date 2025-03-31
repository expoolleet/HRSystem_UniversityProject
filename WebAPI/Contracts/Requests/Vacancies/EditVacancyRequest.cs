using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Vacancies;

public class EditVacancyRequest
{
    [Required]
    public required Guid VacancyId { get; init; }
    
    public string? Description { get; init; }
}