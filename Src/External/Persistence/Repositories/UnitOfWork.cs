using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Persistence.Outbox;
using SharedKernal.Abstraction;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Persistence.Repositories;

internal sealed class UnitOfWork(ApplicationDbContext context) 
    : IUnitOfWork, IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken); ;
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
