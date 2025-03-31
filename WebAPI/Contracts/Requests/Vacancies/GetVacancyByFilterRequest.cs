using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Vacancies;

public class GetVacancyByFilterRequest
{
    [Required]
    public Guid CompnayId { get; init; }
    
    public string? Title { get; init; }
}