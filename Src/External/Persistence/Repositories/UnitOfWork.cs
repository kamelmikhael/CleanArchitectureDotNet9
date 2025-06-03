using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence.Outbox;
using SharedKernal.Abstraction;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Persistence.Repositories;

internal sealed class UnitOfWork(ApplicationDbContext context) 
    : IUnitOfWork, IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChangesCalled();

        return context.SaveChangesAsync(cancellationToken); ;
    }

    public int SaveChanges()
    {
        OnBeforeSaveChangesCalled();

        return context.SaveChanges();
    }

    public void Dispose() 
        => context.Dispose();

    private void OnBeforeSaveChangesCalled()
    {
        ConvertDomainEventsToOutboxMessages();

        UpdateAuditableEntities();
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var outboxMessages = context.ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents.ToList();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().Name ?? string.Empty,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }),
                OccurredOnUtc = DateTime.UtcNow,
                ProcessedOnUtc = null,
                Error = null,
            })
            .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }

    private void UpdateAuditableEntities()
    {
        var utcNow = DateTime.UtcNow;

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
