namespace WebApi.Contracts.Dto.Vacancies;

public class VacancyShortDto
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; init; }
    public required string Description { get;  init; }
}