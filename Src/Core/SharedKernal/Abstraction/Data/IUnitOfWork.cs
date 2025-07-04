﻿using System.Data;

namespace SharedKernal.Abstractions.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();

    IDbTransaction BeginTransaction();
}
