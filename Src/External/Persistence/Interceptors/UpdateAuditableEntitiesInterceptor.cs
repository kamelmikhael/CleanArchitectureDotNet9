using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernal.Abstraction;

namespace Persistence.Interceptors;

internal sealed class UpdateAuditableEntitiesInterceptor
    : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, 
        int result, 
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is not null)
        {
            UpdateAuditableEntities(dbContext);
        }

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext context)
    {
        DateTime utcNow = DateTime.UtcNow;

        _ = context
            .ChangeTracker
            .Entries<IAuditableEntity>()
            .Select(entry =>
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(e => e.CreatedOnUtc).CurrentValue = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.UpdatedOnUtc).CurrentValue = utcNow;
                }

                return entry;
            }).ToList();
    }
}
