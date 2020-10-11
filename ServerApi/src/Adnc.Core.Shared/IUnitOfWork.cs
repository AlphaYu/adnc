using System;
using System.Data;

namespace Adnc.Core.Shared
{
    public interface IUnitOfWork: IDisposable
    {
        string ProviderName { get; }

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void Rollback();

        void Commit();

        //IDbContextTransaction BeginTransaction();

        //IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);

        //Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        //Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    }
}
