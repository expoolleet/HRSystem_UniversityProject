using Application.Companies.Models.Commands;
using Application.Companies.Repositories;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.CommandHandlers;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;

    public CreateCompanyCommandHandler(ICompanyRepository companyRepository)
    {
        ArgumentNullException.ThrowIfNull(companyRepository);
        _companyRepository = companyRepository;
    }

    public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = Company.Create(request.Name);
        await _companyRepository.Create(company, cancellationToken);
        return company.Id;
    }
}