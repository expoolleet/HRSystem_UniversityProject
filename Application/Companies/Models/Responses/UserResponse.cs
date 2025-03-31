using Application.Companies.Models.Submodels;

namespace Application.Companies.Models.Response.Responses;

public class UserResponse
{
    public Guid UserId { get; private init; }
    public Role Role { get; private init; }
    public Token Token { get; private init; }

    private UserResponse(Guid userId, Role role, Token token)
    {
        UserId = userId;
        Role = role;
        Token = token;
    }

    public static UserResponse Create(Guid userId, string roleName, Guid roleId, Token token)
        => new(userId, Role.Create(roleId, roleName), token);
}