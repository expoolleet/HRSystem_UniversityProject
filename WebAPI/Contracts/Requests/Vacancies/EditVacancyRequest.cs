using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Vacancies;

public class EditVacancyRequest
{
    public string? Description { get; init; }
}