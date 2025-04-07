using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Vacancies;

public class GetVacancyByFilterRequest
{
    public Guid? CompanyId { get; init; }
    
    public string? Title { get; init; }
}