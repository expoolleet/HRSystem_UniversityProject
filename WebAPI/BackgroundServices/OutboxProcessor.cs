using Application.DomainEvents;
using Domain.Events;
using Infrastructure.DbContexts;
using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Abstractions.Services;

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
                await ProcessOutboxEvents(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while processing outbox messages");
            }
            await Task.Delay(Delay, stoppingToken);
        }
    }

    private async Task ProcessOutboxEvents(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        var producer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();

        var events = await context.OutboxEvents
                .Where(e => e.ProcessedOn == null)
                .OrderBy(e => e.OccurredOn)
                .Take(BatchSize)
                .ToListAsync(stoppingToken);
        foreach (var @event in events)
        {
            try
            {
                var domainEvent = DeserializeDomainEvent(@event);
                if (domainEvent != null)
                {
                    var domainEventType = Type.GetType(@event.Type);;
                    if (domainEventType != null)
                    {
                        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEventType);
                        var notificationInstance = Activator.CreateInstance(notificationType, domainEvent);
                        if (notificationInstance is INotification notification)
                        {
                            var message = SerializeNotification(notification);
                            var type = notification.GetType().AssemblyQualifiedName;
                            await producer.PublishAsync(type!, message, stoppingToken);
                            _logger.LogInformation(
                                "Domain event {DomainEvent} published to RabbitMQ queue successfully", 
                                domainEvent.GetType().Name);
                        }
                    }
                }
                @event.ProcessedOn = DateTime.UtcNow;
                await context.SaveChangesAsync(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while processing outbox message {MessageId}", @event.Id);
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

    private static string SerializeNotification(INotification notification)
    {
        return JsonConvert.SerializeObject(notification);
    }
}