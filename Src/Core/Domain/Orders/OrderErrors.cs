using SharedKernal.Primitives;

namespace Domain.Orders;

public static class OrderErrors
{
    public static Error NotFound(OrderId orderId) => Error.NotFound(
        "Orders.NotFound",
        $"The order with the Id = '{orderId.Value}' was not found");
}
