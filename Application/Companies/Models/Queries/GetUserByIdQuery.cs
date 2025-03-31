using Domain.Companies;
using MediatR;

namespace Application.Companies.Models.Queries;

public class GetUserByIdQuery : IRequest<User>
{
    public Guid Id { get; init; }
}