namespace Application.Contexts;

public interface IRoleContext
{
    string? GetUserRoleName(Guid userId, CancellationToken cancellationToken);
}