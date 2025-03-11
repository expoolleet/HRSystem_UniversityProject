using Application.Companies.Models.Response;
using Application.Companies.Models.Response.Responses;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Models.Commands;

public class AuthorizeUserCommand : IRequest<UserResponse>
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}