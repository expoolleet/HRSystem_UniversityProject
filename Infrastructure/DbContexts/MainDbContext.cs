using Application.DomainEvents;
using Domain.Abstractions;
using Domain.Candidates;
using Domain.Companies;
using Domain.Events;
using Domain.Vacancies;
using Infrastructure.DbContexts.Configs;
using Infrastructure.Outbox;
using MediatR;

namespace Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Vacancy> Vacancies { get; init; }
    public DbSet<Candidate> Candidates { get; init; }
    public DbSet<Company> Companies { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<OutboxEvent> OutboxEvents { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
        modelBuilder.Entity<User>().Ignore(x => x.DomainEvents);
        modelBuilder.Entity<Vacancy>().Ignore(x => x.DomainEvents);
        modelBuilder.Entity<Candidate>().Ignore(x => x.DomainEvents);
        modelBuilder.Entity<Company>().Ignore(x => x.DomainEvents);
    }
}