using Application.Companies.Models.Responses;
using MediatR;

namespace Application.Companies.Models.Commands;

public class AuthorizeUserCommand : IRequest<UserResponse>
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}