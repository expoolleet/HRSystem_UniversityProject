using Application.DomainEvents;
using Domain.Abstractions;
using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IAuditHelper _auditHelper;
    private readonly IMediator _mediator;

    public AuditSaveChangesInterceptor(IAuditHelper auditHelper, IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(auditHelper);
        ArgumentNullException.ThrowIfNull(mediator);
        _auditHelper = auditHelper;
        _mediator = mediator;
    }

    private async Task ProcessDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        var events = context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var @event in events)
        {
            var outboxEvent = OutboxEvent.Create(@event);
            await context.Set<OutboxEvent>()
                .AddAsync(outboxEvent, cancellationToken);
        }
        
        foreach (var entityEntry in context.ChangeTracker.Entries<Entity>())
        {
            entityEntry.Entity.ClearDomainEvents();
        }
    }
    
    private void ProcessDomainEvents(DbContext context)
    {
        var events = context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var @event in events)
        {
            var outboxEvent = OutboxEvent.Create(@event);
            context.Set<OutboxEvent>()
                .Add(outboxEvent);
        }
        
        foreach (var entityEntry in context.ChangeTracker.Entries<Entity>())
        {
            entityEntry.Entity.ClearDomainEvents();
        }
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData? eventData, 
        InterceptionResult<int> result)
    {
        if (eventData?.Context == null)
        {
            return InterceptionResult<int>.SuppressWithResult(-1);
        }
        ProcessDomainEvents(eventData.Context);
        _auditHelper.UpdateAuditProperties(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData?.Context == null)
        {
            return new ValueTask<InterceptionResult<int>>(InterceptionResult<int>.SuppressWithResult(-1));
        }
        ProcessDomainEventsAsync(eventData.Context, cancellationToken).GetAwaiter().GetResult();
        _auditHelper.UpdateAuditProperties(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}