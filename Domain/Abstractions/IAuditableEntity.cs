namespace Domain.Abstractions;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; init; }
    DateTime? UpdatedAt { get; set; }
}