using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Adnc.Core.Shared;

namespace Adnc.Infr.EfCore
{
    public sealed class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private IDbContextTransaction _dbTransaction;

        public UnitOfWork(TDbContext context)
        {
            this.IsStartingUow = false;
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public dynamic GetDbContextTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (this.IsStartingUow)
                throw new Exception("UnitOfWork Error");
            else
                this.IsStartingUow = true;

            return _dbContext.Database.BeginTransaction(isolationLevel);
        }

        public string ProviderName => _dbContext.Database.ProviderName;

        public bool IsStartingUow { get; private set; }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _dbTransaction = GetDbContextTransaction(isolationLevel);
        }

        public void Commit()
        {
            _dbTransaction?.Commit();
            this.IsStartingUow = false;
        }

        public void Rollback()
        {
            _dbTransaction?.Rollback();
            this.IsStartingUow = false;
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
            this.IsStartingUow = false;
        }
    }
}
