using Application.Companies.Models.Queries;
using Application.Companies.Repositories;
using Domain.Companies;
using MediatR;

namespace Application.Companies.Handlers.QueryHandlers;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Company>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyByIdQueryHandler(ICompanyRepository companyRepository)
    {
        ArgumentNullException.ThrowIfNull(companyRepository);
        _companyRepository = companyRepository;
    }

    public async Task<Company> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _companyRepository.Get(request.Id, cancellationToken);
    }
}