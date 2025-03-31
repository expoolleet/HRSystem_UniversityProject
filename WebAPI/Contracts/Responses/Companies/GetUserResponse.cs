namespace WebApi.Contracts.Responses.Companies;

public class GetUserResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid RoleId { get; set; }
    public Guid CompanyId { get; set; }
}