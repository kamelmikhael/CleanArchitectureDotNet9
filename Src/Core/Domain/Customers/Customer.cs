using SharedKernal.Primitives;

namespace Domain.Customers;

public class Customer : Entity<CustomerId>
{
    private Customer()
    { }

    private Customer(CustomerId id) : base(id) 
    { }

    public string Email { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public static Customer Create(string email, string name)
        => new(new(Guid.NewGuid())) 
        {
            Email = email,
            Name = name
        };
}
