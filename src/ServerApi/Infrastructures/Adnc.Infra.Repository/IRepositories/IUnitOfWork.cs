using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Infra.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsStartingUow { get; }

        [Obsolete("已经废弃，请使用BeginTransaction")]
        dynamic GetDbContextTransaction() { throw new Exception("已经放弃，请使用BeginTransaction"); }

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead, bool sharedToCap = false);

        void Rollback();

        void Commit();

        Task RollbackAsync(CancellationToken cancellationToken = default);

        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}