using Application.Companies.Models.Commands;
using Application.Companies.Repositories;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository)
    {
        ArgumentNullException.ThrowIfNull(roleRepository);
        _roleRepository = roleRepository;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = Role.Create(request.Name);
        await _roleRepository.Create(role, cancellationToken);
        return role.Id;
    }
}