using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernal.Abstractions.Data;

namespace Persistence.Repositories;

internal sealed class UnitOfWork(ApplicationDbContext context) 
    : IUnitOfWork, IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return context.SaveChanges();
    }

    public IDbTransaction BeginTransaction()
    {
        IDbContextTransaction transaction = context.Database.BeginTransaction();

        return transaction.GetDbTransaction();
    }

    public void Dispose() 
        => context.Dispose();
}
