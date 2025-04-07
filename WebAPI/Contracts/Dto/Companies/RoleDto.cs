namespace WebApi.Contracts.Dto.Companies;

public class RoleDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}