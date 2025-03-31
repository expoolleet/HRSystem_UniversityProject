namespace Application.Companies.Models.Submodels;

public class Role
{
    public Guid RoleId { get; private init; }
    
    public string RoleName { get; private init; }

    private Role(Guid roleId, string roleName)
    {
        ArgumentException.ThrowIfNullOrEmpty(roleName);
        RoleId = roleId;
        RoleName = roleName;
    }

    public static Role Create(Guid roleId, string roleName)
        => new(roleId, roleName);
}