namespace Domain.Companies;

public sealed class Role
{
    private Role(Guid id, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Id = id;
        Name = name;
    }

    public Guid Id { get; private init; }
    public string Name { get; private init; }

    public static Role Create(string name)
        => new(Guid.NewGuid(), name);
}
