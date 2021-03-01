using System;
using System.Data;
using Adnc.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Adnc.Infr.EfCore
{
    public sealed class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private IDbContextTransaction _dbTransaction;
        private UnitOfWorkStatus _unitOfWorkStatus;

        public UnitOfWork(TDbContext context, UnitOfWorkStatus unitOfWorkStatus)
        {
            _unitOfWorkStatus = unitOfWorkStatus;
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public dynamic GetDbContextTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_unitOfWorkStatus.IsStartingUow)
                throw new Exception("UnitOfWork Error");
            else
                _unitOfWorkStatus.IsStartingUow = true;

            return _dbContext.Database.BeginTransaction(isolationLevel);
        }

        public string ProviderName => _dbContext.Database.ProviderName;

        public bool IsStartingUow { get { return _unitOfWorkStatus.IsStartingUow; } }

        public bool SharedToCap { get; set; }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _dbTransaction = GetDbContextTransaction(isolationLevel);
        }

        public void Commit()
        {
            _dbTransaction?.Commit();
            _unitOfWorkStatus.IsStartingUow = false;
        }

        public void Rollback()
        {
            _dbTransaction?.Rollback();
            _unitOfWorkStatus.IsStartingUow = false;
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
            if (_unitOfWorkStatus != null)
                _unitOfWorkStatus.IsStartingUow = false;
        }
    }
}
