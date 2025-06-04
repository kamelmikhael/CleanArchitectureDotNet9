using Application.Users.Register;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace MessageBroker.Consumers;

public sealed class UserRegisteredEventBusConsumer(
    ILogger<UserRegisteredEventBusConsumer> logger) : IConsumer<UserRegisteredEventBus>
{
    public Task Consume(ConsumeContext<UserRegisteredEventBus> context)
    {
        logger.LogInformation("User registered: {@User}", context.Message);

        return Task.CompletedTask;
    }
}
