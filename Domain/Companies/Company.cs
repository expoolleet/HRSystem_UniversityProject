using Domain.Abstractions;

namespace Domain.Companies;

public sealed class Company : Entity
{
    private Company(Guid id, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Id = id;
        Name = name;
    }

    public Guid Id { get; private init; }
    public string Name { get; private init; }

    public static Company Create(string name)
    {
        return new Company(Guid.NewGuid(), name);
    }
}
