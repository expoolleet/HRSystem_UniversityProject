using Domain.Companies;
using MediatR;

namespace Application.Companies.Models.Queries;

public class GetUserByLoginQuery : IRequest<User>
{
    public required string Login { get; init; }
}