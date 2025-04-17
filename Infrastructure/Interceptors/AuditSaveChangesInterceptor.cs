using Application.DomainEvents;
using Domain.Abstractions;
using MediatR;
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
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context == null)
        {
            return base.SavedChanges(eventData, result);
        }
        var events = eventData.Context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();
        
        foreach (var entityEntry in eventData.Context.ChangeTracker.Entries<Entity>())
        {
            entityEntry.Entity.ClearDomainEvents();
        }

        foreach (var @event in events)
        {
            var domainEventType = @event.GetType();
            var notificationWrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEventType);
            var notificationWrapperInstance = Activator.CreateInstance(notificationWrapperType, @event);
            if (notificationWrapperInstance is INotification notification)
            {
                _mediator.Publish(notification).GetAwaiter().GetResult();
            }
        }
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context == null)
        {
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
        var events = eventData.Context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();
        
        foreach (var entityEntry in eventData.Context.ChangeTracker.Entries<Entity>())
        {
            entityEntry.Entity.ClearDomainEvents();
        }

        foreach (var @event in events)
        {
            var domainEventType = @event.GetType();
            var notificationWrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEventType);
            var notificationWrapperInstance = Activator.CreateInstance(notificationWrapperType, @event);
            if (notificationWrapperInstance is INotification notification)
            {
                await _mediator.Publish(notification, cancellationToken);
            }
        }
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData? eventData, InterceptionResult<int> result)
    {
        if (eventData?.Context == null)
        {
            return InterceptionResult<int>.SuppressWithResult(-1);
        }
            
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
        _auditHelper.UpdateAuditProperties(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}