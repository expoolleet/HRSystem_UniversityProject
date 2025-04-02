using MediatR;

namespace Application.Companies.Models.Commands;

public class CreateCompanyCommand : IRequest<Guid>
{
    public required string Name { get; init; }
}