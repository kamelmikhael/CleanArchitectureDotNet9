using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernal.Abstraction;
using SharedKernal.Abstractions;

namespace Infrastructure.Services;

internal sealed class DomainEventPublisher(
    ILogger<DomainEventPublisher> logger,
    IServiceProvider serviceProvider) : IDomainEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        logger.LogInformation("Event published: {@Name} at {Time} UTC",
            domainEvent.GetType().Name,
            DateTime.UtcNow);

        var handlers = serviceProvider
           .GetServices<IDomainEventHandler<TEvent>>()
           .Where(h => h is not null)
           .ToList();

        if (handlers.Count == 0)
        {
            return;
        }

        //var tasks = handlers
        //    .Select(handler => handler.HandleAsync(domainEvent, cancellationToken))
        //    .ToList();

        //await Task.WhenAll(tasks);

        foreach (IDomainEventHandler<TEvent> handler in handlers)
        {
            await handler.HandleAsync(domainEvent, cancellationToken);
        }
    }
}
