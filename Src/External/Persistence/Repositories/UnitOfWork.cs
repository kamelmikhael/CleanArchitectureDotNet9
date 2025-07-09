using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernal.Abstractions.Data;

namespace Persistence.Repositories;

internal sealed class UnitOfWork(ApplicationDbContext context) 
    : IUnitOfWork, IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);

    public int SaveChanges()
        => context.SaveChanges();

    public IDbTransaction BeginTransaction()
        => context.Database.BeginTransaction().GetDbTransaction();

    public void Dispose() 
        => context.Dispose();
}
