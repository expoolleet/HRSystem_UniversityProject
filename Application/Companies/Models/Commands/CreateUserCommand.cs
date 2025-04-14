using MediatR;

namespace Application.Companies.Models.Commands;

public class CreateUserCommand : IRequest<Guid>
{
    public Guid RoleId { get; init; } 
    public Guid CompanyId { get; init; }
    public required string Password { get; init; }
    public required string Name { get; init; }
}