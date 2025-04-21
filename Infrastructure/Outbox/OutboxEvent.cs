using Domain.Events;
using Newtonsoft.Json;

namespace Infrastructure.Outbox;

public class OutboxEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
    public string Type { get; private set; }
    public string Content { get; private set; }
    public DateTime? ProcessedOn { get; set; }

    private OutboxEvent() { }

    private OutboxEvent(Guid id, string type, string content, DateTime occurredOn)
    {
        Id = id;
        Type = type;
        Content = content;
        OccurredOn = occurredOn;
    }
    
    public static OutboxEvent Create(IDomainEvent domainEvent)
     => new(Guid.NewGuid(), 
         domainEvent.GetType().AssemblyQualifiedName!, 
         JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }), 
         domainEvent.OccurredOn);
}