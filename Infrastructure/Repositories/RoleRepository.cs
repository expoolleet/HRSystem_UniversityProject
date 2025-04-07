using Application.Companies.Repositories;
using Domain.Companies;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly MainDbContext _dbContext;
    public RoleRepository(MainDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }
    
    public async Task Create(Role role, CancellationToken cancellationToken)
    {
        await _dbContext.Roles.AddAsync(role, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Role> Get(Guid id, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        return role!;
    }

    public async Task<Role> Get(string name, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        return role!;
    }
}