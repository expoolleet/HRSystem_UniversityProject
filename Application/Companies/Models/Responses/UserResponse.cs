using Application.Companies.Models.Submodels;
using Domain.Companies;

namespace Application.Companies.Models.Responses;

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

    public static UserResponse Create(Guid userId, Role role, Token token)
        => new(userId, role, token);
}