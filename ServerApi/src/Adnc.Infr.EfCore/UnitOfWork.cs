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
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private IDbContextTransaction _dbTransaction;

        public UnitOfWork(TDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string ProviderName => _dbContext.Database.ProviderName;


        public void BeginTransaction()
        {
            _dbTransaction = _dbContext.Database.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _dbTransaction = _dbContext.Database.BeginTransaction(isolationLevel);
        }


        public void Commit()
        {
            _dbTransaction?.Commit();
        }

        public void Rollback()
        {
            _dbTransaction?.Rollback();
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
        }
    }
}
