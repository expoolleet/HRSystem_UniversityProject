namespace Domain.Companies;

public sealed class User
{
    private User(Guid id, Guid roleId, Guid companyId, string name, Password password)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Id = id;
        RoleId = roleId;
        CompanyId = companyId;
        Name = name;
        Password = password;
    }

    public Guid Id { get; private init; }
    public Guid RoleId { get; private init; } 
    public Guid CompanyId { get; private init; }
    public Password Password { get; private init; }
    public string Name { get; private init; }

    public static User Create(Guid roleId, Guid companyId, string name, string password)
        => new(Guid.NewGuid(), roleId, companyId, name, Password.Create(password));
}
