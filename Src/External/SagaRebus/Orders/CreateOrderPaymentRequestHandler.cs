using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;

namespace SagaRebus.Orders;

internal sealed class CreateOrderPaymentRequestHandler(
    ILogger<CreateOrderPaymentRequestHandler> logger,
    IBus bus)
    : IHandleMessages<CreateOrderPaymentRequest>
{
    public async Task Handle(CreateOrderPaymentRequest message)
    {
        logger.LogInformation("Start payment request {@OrderId}, at {@Time} UTC"
            , message.OrderId
            , DateTime.UtcNow);

        await Task.Delay(2000);

        logger.LogInformation("Payment request started {@OrderId}, at {@Time} UTC"
            , message.OrderId
            , DateTime.UtcNow);

        await bus.Send(new OrderPaymentRequestSent(message.OrderId));
    }
}
