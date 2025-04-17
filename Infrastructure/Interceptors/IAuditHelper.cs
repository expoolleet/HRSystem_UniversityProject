using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Interceptors;

public interface IAuditHelper
{
    void UpdateAuditProperties(DbContext dbContext);
}