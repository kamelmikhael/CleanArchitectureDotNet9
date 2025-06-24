using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Customers;

internal sealed class CustomerRepository(ApplicationDbContext context)
    : Repository<Customer, CustomerId>(context), ICustomerRepository
{
    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await _dbSet.AnyAsync(c => c.Email == email, cancellationToken: cancellationToken);
    }
}
