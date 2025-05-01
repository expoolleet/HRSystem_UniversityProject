using System.Text;
using Application.DomainEvents;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WebApi.Services;
using WebApi.Settings;

namespace WebApi.BackgroundServices;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly IOptions<RabbitMqSettings> _settings;
    private readonly RabbitMqInitializer _initializer;
    private readonly IChannel _channel;

    public RabbitMqConsumer(
        IServiceProvider serviceProvider, 
        IChannel channel, 
        IOptions<RabbitMqSettings> settings, 
        RabbitMqInitializer initializer,  
        ILogger<RabbitMqConsumer> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(initializer);
        _serviceProvider = serviceProvider;
        _channel = channel;
        _logger = logger;
        _settings = settings;
        _initializer = initializer;
    }
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting RabbitMQ consumer");
        try
        {
            var messageCount = await _channel.QueuePurgeAsync(_settings.Value.QueueName, cancellationToken);
            _logger.LogInformation("Purged {MessageCount} messages from the queue {QueueName}", messageCount,
                _settings.Value.QueueName);
            
            await _initializer.InitializeAsync(_channel, cancellationToken);
            
            await _channel.QueueDeclareAsync(
                queue: _settings.Value.QueueName, 
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);
            
            await _channel.QueueBindAsync(
                queue: _settings.Value.QueueName, 
                exchange: _settings.Value.Exchange, 
                routingKey: _settings.Value.RoutingKey, 
                cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while purging queue");
        }
        finally
        {
            await base.StartAsync(cancellationToken);
        }
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var channel = scope.ServiceProvider.GetRequiredService<IChannel>();
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var typeName = eventArgs.BasicProperties.Type;
                if (typeName == null)
                { 
                    throw new Exception("Message type is empty");
                }
                var type = Type.GetType(typeName);
                if (type == null)
                {
                    throw new Exception("Message type is not found");
                }
                var bodyString = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject(bodyString, type);
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                if (message is not INotification notification)
                {
                    if (message is IDomainEvent domainEvent)
                    {
                        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
                        notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;
                    }
                    else
                    {
                        throw new Exception("Message is not INotification or IDomainEvent");
                    }
                }
                await mediator.Publish(notification, stoppingToken);
                var ackChannel = scope.ServiceProvider.GetRequiredService<IChannel>();
                await ackChannel.BasicAckAsync(
                    eventArgs.DeliveryTag, 
                    multiple: false, 
                    cancellationToken: stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while processing message");
            }
        };
        await channel.BasicConsumeAsync(
            queue: _settings.Value.QueueName, 
            autoAck: false, 
            consumer: consumer, 
            cancellationToken: stoppingToken);
    }
}