using Domain.Companies;
using MediatR;

namespace Application.Companies.Models.Commands;

public class DeleteUserCommand : IRequest<User>
{
    public Guid Id { get; set; }
}