using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence;
using Persistence.Outbox;
using Polly;
using Quartz;
using SharedKernal.Abstraction;
using SharedKernal.Abstractions;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEventPublisher _eventPublisher;

    public ProcessOutboxMessagesJob(
        ApplicationDbContext dbContext,
        IEventPublisher eventPublisher)
    {
        _dbContext = dbContext;
        _eventPublisher = eventPublisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var message in messages)
        {
            IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content);

            if (domainEvent is null) continue;

            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    3, 
                    attempt => TimeSpan.FromMilliseconds(50 * attempt));

            var result = await policy.ExecuteAndCaptureAsync(() => 
                _eventPublisher.PublishAsync(
                    domainEvent, 
                    context.CancellationToken));

            // Log the error and set the error message
            message.Error = result.FinalException?.ToString();

            // Mark the message as processed
            message.ProcessedOnUtc = DateTime.UtcNow;

            _dbContext.Update(message);
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
