using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernal.Abstraction;

namespace Persistence.Interceptors;

public sealed class UpdateAuditableEntitiesInterceptor
    : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, 
        int result, 
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        var utcNow = DateTime.UtcNow;

        _ = dbContext
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

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
