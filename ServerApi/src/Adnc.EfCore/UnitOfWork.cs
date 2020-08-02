using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Core;

namespace  Adnc.Infr.EfCore
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

        public IDbContextTransaction CurrentTransaction => _database.CurrentTransaction;

        public bool AutoTransactionsEnabled => false;

        public string ProviderName => _database.ProviderName;

        public IDbContextTransaction BeginTransaction()=> _database.BeginTransaction();

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => _database.BeginTransactionAsync(cancellationToken);

        public bool CanConnect() => _database.CanConnect();

        public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default) => _database.CanConnectAsync(cancellationToken);

        public void CommitTransaction() => _database.CommitTransaction();

        public void RollbackTransaction() => _database.RollbackTransaction();
    }
}
