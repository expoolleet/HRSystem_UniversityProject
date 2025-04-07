namespace Application.Contexts;

public interface IUserContext
{
    Task<Guid?> GetUserId(CancellationToken cancellationToken);
}