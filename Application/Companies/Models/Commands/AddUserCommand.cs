using Domain.Companies;
using MediatR;

namespace Application.Companies.Models.Commands;

public class AddUserCommand : IRequest
{
    public required User User { get; set; }
}