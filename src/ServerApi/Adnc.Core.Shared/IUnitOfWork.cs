using System;
using System.Data;

namespace Adnc.Core.Shared
{
    public interface IUnitOfWork: IDisposable
    {
        [Obsolete("还没有相到好的方案")]
        bool IsStartingUow { get;}

        [Obsolete("还没有相到好的方案")]
        bool SharedToCap { get; set; }

        string ProviderName { get; }

        dynamic GetDbContextTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void Rollback();

        void Commit();

        //IDbContextTransaction BeginTransaction();

        //IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);

        //Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        //Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    }
}
