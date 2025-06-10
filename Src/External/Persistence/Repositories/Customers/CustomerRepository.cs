using Domain.Customers;

namespace Persistence.Repositories.Customers;

internal sealed class CustomerRepository(ApplicationDbContext context)
    : Repository<Customer, CustomerId>(context), ICustomerRepository
{

}
