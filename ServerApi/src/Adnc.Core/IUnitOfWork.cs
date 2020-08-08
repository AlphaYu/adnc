using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Core
{
    public interface IUnitOfWork
    {
        string ProviderName { get; }

        IDbContextTransaction BeginTransaction();

        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    }
}
