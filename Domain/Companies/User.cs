namespace Domain.Companies;

public sealed class User
{
    private User(Guid id, Guid roleId, string name, Password password)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Id = id;
        RoleId = roleId;
        Name = name;
        Password = password;
    }

    public Guid Id { get; private init; }
    public Guid RoleId { get; private init; }
    public Password Password { get; private init; }
    public string Name { get; private init; }

    public static User Create(Guid roleId, string name, string password)
        => new(Guid.NewGuid(), roleId, name, Password.Create(password));
}
