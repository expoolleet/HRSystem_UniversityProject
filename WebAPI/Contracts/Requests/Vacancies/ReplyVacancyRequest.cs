using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Vacancies;

public class ReplyVacancyRequest
{
    [Required]
    public Guid VacancyId { get; set; }
    
    public Guid? ReferalId { get; set; }
    
    [Required]
    [MaxLength(30)]
    public required string Name { get; init; }
    
    [MaxLength(100)]
    public string? Portfolio { get; init; }
    
    [Required]
    public int WorkExperience { get; init; }
}