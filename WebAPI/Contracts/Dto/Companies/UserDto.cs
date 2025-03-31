namespace WebApi.Contracts.Dto.Companies;

public class UserDto
{
    public Guid Id { get; init; }
    public Guid RoleId { get; init; }
    public Guid CompanyId { get; init; }
    public required string Name { get; init; }
}