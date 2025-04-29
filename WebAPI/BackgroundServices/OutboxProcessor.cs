using Application.DomainEvents;
using Domain.Events;
using Infrastructure.DbContexts;
using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApi.BackgroundServices;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxProcessor> _logger;
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(10);
    private const int BatchSize = 20;
    
    public OutboxProcessor(IServiceProvider serviceProvider, ILogger<OutboxProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessages(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while processing outbox messages");
            }
            await Task.Delay(Delay, stoppingToken);
        }
    }

    private async Task ProcessOutboxMessages(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var messages = await context.OutboxEvents
                .Where(e => e.ProcessedOn == null)
                .OrderBy(e => e.OccurredOn)
                .Take(BatchSize)
                .ToListAsync(stoppingToken);
        foreach (var message in messages)
        {
            try
            {
                var domainEvent = DeserializeDomainEvent(message);
                if (domainEvent != null)
                {
                    var domainEventType = Type.GetType(message.Type);;
                    if (domainEventType != null)
                    {
                        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEventType);
                        var notificationInstance = Activator.CreateInstance(notificationType, domainEvent);
                        if (notificationInstance is INotification notification)
                        {
                            await mediator.Publish(notification, stoppingToken);
                            _logger.LogInformation("Domain event {DomainEvent} published", domainEvent.GetType().Name);
                        }
                    }
                }
                message.ProcessedOn = DateTime.UtcNow;
                await context.SaveChangesAsync(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while processing outbox message {MessageId}", message.Id);
            }
        }
    }

    private static IDomainEvent? DeserializeDomainEvent(OutboxEvent outboxEvent)
    {
        var type = Type.GetType(outboxEvent.Type);
        if (type == null)
        {
            return null;
        }
        return JsonConvert.DeserializeObject(outboxEvent.Content, type) as IDomainEvent;
    }
}