using SharedKernal.Abstraction.Data;

namespace Domain.Customers;

public interface ICustomerRepository : IRepository<Customer, CustomerId>
{
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
}
