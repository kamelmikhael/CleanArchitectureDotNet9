using Application.Orders.Create;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using IntegrationEvents.Orders;

namespace SagaRebus.Orders;

internal sealed record OrderConfirmationEmailSent(Guid OrderId);
internal sealed record OrderPaymentRequestSent(Guid OrderId);

internal class OrderCreateSaga
    : Saga<OrderCreateSagaData>
    , IAmInitiatedBy<OrderCreatedIntegrationEvent>
    , IHandleMessages<OrderConfirmationEmailSent>
    , IHandleMessages<OrderPaymentRequestSent>
{
    /// <summary>
    /// To send messages over the queue
    /// </summary>
    private readonly IBus _bus;

    public OrderCreateSaga(IBus bus)
        => _bus = bus;

    protected override void CorrelateMessages(ICorrelationConfig<OrderCreateSagaData> config)
    {
        config.Correlate<OrderCreatedIntegrationEvent>(m => m.OrderId, sagaData => sagaData.OrderId);
        config.Correlate<OrderConfirmationEmailSent>(m => m.OrderId, sagaData => sagaData.OrderId);
        config.Correlate<OrderPaymentRequestSent>(m => m.OrderId, sagaData => sagaData.OrderId);
    }

    public async Task Handle(OrderCreatedIntegrationEvent message)
    {
        if (!IsNew)
        {
            return;
        }

        await _bus.Send(new SendOrderConfirmationEmail(message.OrderId));
    }

    public async Task Handle(OrderConfirmationEmailSent message)
    {
        Data.ConfirmationEmailSent = true;

        await _bus.Send(new CreateOrderPaymentRequest(message.OrderId));
    }

    public Task Handle(OrderPaymentRequestSent message)
    {
        Data.PaymentRequestSent = true;

        MarkAsComplete();

        return Task.CompletedTask;
    }
}
