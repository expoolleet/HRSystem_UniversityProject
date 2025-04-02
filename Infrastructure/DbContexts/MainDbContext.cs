using System.Data.Common;
using Domain.Candidates;
using Domain.Companies;
using Domain.Vacancies;
using Infrastructure.DbContexts.Configs;
using Microsoft.Data.Sqlite;

namespace Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Vacancy> Vacancies { get; init; }
    public DbSet<Candidate> Candidates { get; init; }
    public DbSet<Company> Companies { get; init; }
    public DbSet<Role> Roles { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}