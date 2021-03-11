﻿using System;
using System.Data;
using DotNetCore.CAP;
using Adnc.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using System.Threading;

namespace Adnc.Infr.EfCore
{
    public sealed class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly UnitOfWorkStatus _unitOfWorkStatus;
        private readonly ICapPublisher _capPublisher;
        private IDbContextTransaction _dbTransaction;

        public bool IsStartingUow { get { return _unitOfWorkStatus.IsStartingUow; } }

        public UnitOfWork(TDbContext context
            , UnitOfWorkStatus unitOfWorkStatus
            , ICapPublisher capPublisher = null)
        {
            _unitOfWorkStatus = unitOfWorkStatus;
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _capPublisher = capPublisher;
        }

        private IDbContextTransaction GetDbContextTransaction(IsolationLevel isolationLevel, bool sharedToCap = false)
        {
            if (_unitOfWorkStatus.IsStartingUow)
                throw new Exception("UnitOfWork Error");
            else
                _unitOfWorkStatus.IsStartingUow = true;

            IDbContextTransaction trans;

            if (sharedToCap)
            {
                if (_capPublisher == null)
                    throw new Exception("CapPublisher is null");
                else
                    trans = _dbContext.Database.BeginTransaction(_capPublisher, false);
            }
            else
                trans = _dbContext.Database.BeginTransaction(isolationLevel);

            return trans;
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead, bool sharedToCap = false)
        {
            _dbTransaction = GetDbContextTransaction(isolationLevel, sharedToCap);
        }

        public void Commit()
        {
            CheckNotNull(_dbTransaction);

            _dbTransaction.Commit();
            _unitOfWorkStatus.IsStartingUow = false;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            CheckNotNull(_dbTransaction);

            await _dbTransaction.CommitAsync(cancellationToken);
            _unitOfWorkStatus.IsStartingUow = false;
        }


        public void Rollback()
        {
            CheckNotNull(_dbTransaction);

            _dbTransaction.Rollback();
            _unitOfWorkStatus.IsStartingUow = false;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            CheckNotNull(_dbTransaction);

            await _dbTransaction.RollbackAsync(cancellationToken);
            _unitOfWorkStatus.IsStartingUow = false;
        }

        public void Dispose()
        {
            var isNotNull = CheckNotNull(_dbTransaction, false);
            if (isNotNull)
            {
                _dbTransaction.Dispose();
                if (_unitOfWorkStatus != null)
                    _unitOfWorkStatus.IsStartingUow = false;
            }
        }

        private bool CheckNotNull(IDbContextTransaction dbContextTransaction, bool isThrowException = true)
        {
            if (dbContextTransaction == null && isThrowException)
                throw new Exception("IDbContextTransaction is null");

            return dbContextTransaction != null;
        }
    }
}
