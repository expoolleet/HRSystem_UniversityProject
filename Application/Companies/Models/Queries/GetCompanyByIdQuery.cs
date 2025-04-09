using Domain.Companies;
using MediatR;

namespace Application.Companies.Models.Queries;

public class GetCompanyByIdQuery : IRequest<Company>
{
    public Guid Id { get; init; }
}