using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WebApi.Abstractions.Services;
using WebApi.Settings;

namespace WebApi.Services;

public class RabbitMqProducer : IMessageProducer
{
    private readonly IOptions<RabbitMqSettings> _settings;
    private readonly IChannel _channel;
    private readonly RabbitMqInitializer _initializer;
    private readonly ILogger<RabbitMqProducer> _logger;

    public RabbitMqProducer(
        IOptions<RabbitMqSettings> settings, 
        IChannel channel, 
        RabbitMqInitializer initializer, 
        ILogger<RabbitMqProducer> logger)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(initializer);
        ArgumentNullException.ThrowIfNull(logger);
        _settings = settings;
        _channel = channel;
        _initializer = initializer;
        _logger = logger;
    }
    
    public void Publish(string messageType, string message)
    {
        throw new NotImplementedException();
    }

    public async Task PublishAsync(string messageType, string message, CancellationToken cancellationToken = default)
    {
        await _initializer.InitializeAsync(_channel, cancellationToken);
        _channel.BasicReturnAsync += (sender, eventArgs) =>
        {   
            var messageBody = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            _logger.LogWarning(
                "Message returned from RabbitMQ: {ReplyText}, Message: {Message}, RouteKey: {RoutingKey}", 
                eventArgs.ReplyText, 
                messageBody,
                eventArgs.RoutingKey);
            return Task.CompletedTask;
        };
        var body = Encoding.UTF8.GetBytes(message);

        var props = new BasicProperties
        {
            Type = messageType,
            Persistent = true
        };
        await _channel.BasicPublishAsync(
            exchange: _settings.Value.Exchange,
            routingKey: _settings.Value.RoutingKey,
            mandatory: true,
            basicProperties: props,
            body: body,
            cancellationToken: cancellationToken);
    }
}