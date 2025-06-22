using Domain.Customers;
using Domain.Orders;

namespace Domain.UnitTests.Orders;

public class OrderTests
{
    [Theory]
    [ClassData(typeof(OrderCreateTestData))]
    public void Create_Should_RaiseDomainEvent(CustomerId customerId)
    {
        // Arrange

        // Act
        var order = Order.Create(customerId);

        // Assert
        Assert.NotEmpty(order.DomainEvents.OfType<OrderCreatedDomainEvent>());
    }
}
public class OrderCreateTestData : TheoryData<CustomerId>
{
    public OrderCreateTestData()
    {
        Add(new CustomerId(Guid.NewGuid()));
    }
}
