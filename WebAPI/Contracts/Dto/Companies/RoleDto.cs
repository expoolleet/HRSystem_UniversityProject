namespace WebApi.Contracts.Dto.Companies;

public class RoleDto
{
    public Guid RoleId { get; set; }
    public required string RoleName { get; set; }
}