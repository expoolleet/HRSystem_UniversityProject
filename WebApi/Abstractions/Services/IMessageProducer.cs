namespace WebApi.Abstractions.Services;

public interface IMessageProducer
{
    void Publish(string messageType, string message);
    
    Task PublishAsync(string messageType, string message, CancellationToken cancellationToken = default);
}