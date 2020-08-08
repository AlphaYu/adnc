using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Core;
using System.Data;

namespace Adnc.Infr.EfCore
{
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly DatabaseFacade _database;

        public UnitOfWork(TDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _database = _dbContext.Database;
        }

        public string ProviderName => _database.ProviderName;

        public IDbContextTransaction BeginTransaction() => _database.BeginTransaction();

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel) => _database.BeginTransaction(isolationLevel);

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => _database.BeginTransactionAsync(cancellationToken);

        public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default) => _database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }
}
