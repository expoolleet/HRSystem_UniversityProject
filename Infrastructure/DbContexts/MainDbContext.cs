using System.Data.Common;
using Application.DomainEvents;
using Domain.Candidates;
using Domain.Companies;
using Domain.Entities;
using Domain.Events;
using Domain.Vacancies;
using Infrastructure.DbContexts.Configs;
using MediatR;

namespace Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

public class MainDbContext : DbContext
{
    private readonly IMediator _mediator;
    public DbSet<User> Users { get; init; }
    public DbSet<Vacancy> Vacancies { get; init; }
    public DbSet<Candidate> Candidates { get; init; }
    public DbSet<Company> Companies { get; init; }
    public DbSet<Role> Roles { get; init; }

    public MainDbContext(DbContextOptions<MainDbContext> options, IMediator mediator)
        : base(options)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
        modelBuilder.Entity<Candidate>().Ignore(x => x.DomainEvents);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .SelectMany(entry => entry.Entity.DomainEvents ?? Enumerable.Empty<IDomainEvent>())
            .ToList();
        
        ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents != null && entry.Entity.DomainEvents.Any())
            .ToList()
            .ForEach(entry => entry.Entity.ClearDomainEvents());
        
        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            Type domainEventType = domainEvent.GetType();
            Type notificationWrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEventType);
            object notificationWrapperInstance = Activator.CreateInstance(notificationWrapperType, domainEvent);
            if (notificationWrapperInstance is INotification notification)
            {
                await _mediator.Publish(notification, cancellationToken);
            }
            else
            {
                Console.WriteLine($"Error: Could not wrap domain event of type {domainEventType} for MediatR publication");
            }
        }
        return result;
    }

    public override int SaveChanges()
    {
        return SaveChanges(acceptAllChangesOnSuccess: true);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .SelectMany(entry => entry.Entity.DomainEvents ?? Enumerable.Empty<IDomainEvent>())
            .ToList();
        
        ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents != null && entry.Entity.DomainEvents.Any())
            .ToList()
            .ForEach(entry => entry.Entity.ClearDomainEvents());
        
        var result =  base.SaveChanges(acceptAllChangesOnSuccess);

        foreach (var domainEvent in domainEvents)
        {
            Type domainEventType = domainEvent.GetType();
            Type notificationWrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEventType);
            object notificationWrapperInstance = Activator.CreateInstance(notificationWrapperType, domainEvent);
            if (notificationWrapperInstance is INotification notification)
            {
                 _mediator.Publish(notification);
            }
            else
            {
                Console.WriteLine($"Error: Could not wrap domain event of type {domainEventType} for MediatR publication");
            }
        }
        return result;
    }
}