namespace WebApi.Contracts.Dto.Companies;

public class CompanyDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }

}