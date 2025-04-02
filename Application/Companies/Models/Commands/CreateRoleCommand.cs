using MediatR;

namespace Application.Companies.Models.Commands;

public class CreateRoleCommand :IRequest<Guid>
{
    public required string Name { get; init; }
}