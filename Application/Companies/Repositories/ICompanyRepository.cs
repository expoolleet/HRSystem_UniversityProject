using Domain.Companies;

namespace Application.Companies.Repositories;

public interface ICompanyRepository
{
    Task Create(
        Company company,
        CancellationToken cancellationToken);
    
    Task<Company> Get(
        Guid id, 
        CancellationToken cancellationToken);
    
    Task<Company> Get(
        string name, 
        CancellationToken cancellationToken);
}