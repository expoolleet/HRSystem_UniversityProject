using Application.Companies.Repositories;
using Domain.Companies;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly MainDbContext _dbContext;
    public CompanyRepository(MainDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }
    
    public async Task Create(Company company, CancellationToken cancellationToken)
    {
        await _dbContext.Companies.AddAsync(company, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Company> Get(Guid id, CancellationToken cancellationToken)
    {
        var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return company!;
    }

    public async Task<Company> Get(string name, CancellationToken cancellationToken)
    {
        var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
        return company!;
    }
}