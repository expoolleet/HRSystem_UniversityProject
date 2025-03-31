using WebApi.Contracts.Dto.Companies;

namespace WebApi.Contracts.Responses.Companies;

public class AuthUserResponse
{
    public Guid UserId { get; init; }
    public required RoleDto Role { get; init; }
    public required TokenDto Token { get; init; }
}