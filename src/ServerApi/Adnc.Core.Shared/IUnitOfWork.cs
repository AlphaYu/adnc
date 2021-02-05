using System;
using System.Data;

namespace Adnc.Core.Shared
{
    public interface IUnitOfWork: IDisposable
    {
        bool IsStartingUow { get;}

        dynamic GetDbContextTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        string ProviderName { get; }

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void Rollback();

        void Commit();

        //IDbContextTransaction BeginTransaction();

        //IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);

        //Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        //Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    }
}
