using Domain.Candidates;
using Domain.Companies;
using Domain.Vacancies;
using Infrastructure.DbContexts.Configs;

namespace Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

public class MainDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<Candidate> Candidates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
    }
}