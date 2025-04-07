using Domain.Companies;
using MediatR;

namespace Application.Companies.Models.Queries;

public class GetUserByNameQuery : IRequest<User>
{
    public required string Name { get; init; }
}