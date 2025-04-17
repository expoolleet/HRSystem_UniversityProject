using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Interceptors;

public class AuditHelper : IAuditHelper
{
    public void UpdateAuditProperties(DbContext context)
    {
        var now = DateTime.UtcNow;

        var entities = context.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entities)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(e => e.CreatedAt).CurrentValue = now;
                entry.Property(e => e.UpdatedAt).CurrentValue = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(e => e.UpdatedAt).CurrentValue = now;
            }
        }
    }
}