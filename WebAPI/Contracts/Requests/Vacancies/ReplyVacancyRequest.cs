using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Vacancies;

public class ReplyVacancyRequest
{
    [Required]
    [MaxLength(30)]
    public required string Name { get; init; }
    
    [Required]
    [MaxLength(100)]
    public required string Portfolio { get; init; }
    
    [Required]
    public int WorkExperience { get; init; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; init; }
}