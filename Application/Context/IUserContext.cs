namespace Application.Context;

public interface IUserContext
{
    Task<Guid> GetUserId(CancellationToken cancellationToken);
}