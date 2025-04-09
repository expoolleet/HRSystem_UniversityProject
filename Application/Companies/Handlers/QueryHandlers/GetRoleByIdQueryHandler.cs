using Application.Companies.Models.Queries;
using Application.Companies.Repositories;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.QueryHandlers;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Role>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    {
        ArgumentNullException.ThrowIfNull(roleRepository);
        _roleRepository = roleRepository;
    }

    public async Task<Role> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
    {
        return await _roleRepository.Get(query.Id, cancellationToken);
    }
}