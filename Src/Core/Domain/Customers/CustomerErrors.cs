using SharedKernal.Primitives;

namespace Domain.Customers;

public static class CustomerErrors
{
    public static Error NotFound(CustomerId customerId) => Error.NotFound(
        "Customers.NotFound",
        $"The customer with the Id = '{customerId.Value}' was not found");
}
