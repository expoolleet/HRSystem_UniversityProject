using Domain.Companies;
using MediatR;

namespace Application.Companies.Models.Queries;

public class GetRoleByIdQuery : IRequest<Role>
{
    public Guid Id { get; init; }
}