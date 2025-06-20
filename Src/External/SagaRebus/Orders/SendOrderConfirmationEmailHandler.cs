using Application.Orders.Create;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;

namespace SagaRebus.Orders;

internal sealed class SendOrderConfirmationEmailHandler(
    ILogger<SendOrderConfirmationEmailHandler> logger,
    IBus bus)
    : IHandleMessages<SendOrderConfirmationEmail>
{
    public async Task Handle(SendOrderConfirmationEmail message)
    {
        logger.LogInformation("Sending order confirmation email {@OrderId}, at {@Time} UTC"
            , message.OrderId
            , DateTime.UtcNow);

        await Task.Delay(2000);

        logger.LogInformation("Order confirmation email sent {@OrderId}, at {@Time} UTC"
            , message.OrderId
            , DateTime.UtcNow);

        await bus.Send(new OrderConfirmationEmailSent(message.OrderId));
    }
}
