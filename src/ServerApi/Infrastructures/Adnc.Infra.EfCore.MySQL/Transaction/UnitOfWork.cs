using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Adnc.Infra.EfCore.Transaction;

public sealed class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private readonly UnitOfWorkStatus _unitOfWorkStatus;
    private readonly ICapPublisher _capPublisher;
    private IDbContextTransaction _dbTransaction;

    public bool IsStartingUow => _unitOfWorkStatus.IsStartingUow;

    public UnitOfWork(TDbContext context
        , UnitOfWorkStatus unitOfWorkStatus
        , ICapPublisher capPublisher = null)
    {
        _unitOfWorkStatus = unitOfWorkStatus ?? throw new ArgumentNullException(nameof(unitOfWorkStatus));
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        _capPublisher = capPublisher;
    }

    private IDbContextTransaction GetDbContextTransaction(IsolationLevel isolationLevel, bool distributed = false)
    {
        if (_unitOfWorkStatus.IsStartingUow)
            throw new ArgumentException("UnitOfWork Error");
        else
            _unitOfWorkStatus.IsStartingUow = true;

        IDbContextTransaction trans;

        if (distributed)
        {
            if (_capPublisher == null)
                throw new ArgumentException("CapPublisher is null");
            else
                trans = _dbContext.Database.BeginTransaction(_capPublisher, false);
        }
        else
            trans = _dbContext.Database.BeginTransaction(isolationLevel);

        return trans;
    }

    public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool distributed = false)
    {
        _dbTransaction = GetDbContextTransaction(isolationLevel, distributed);
    }

    public void Commit()
    {
        CheckNotNull(_dbTransaction);

        _dbTransaction.Commit();
        _unitOfWorkStatus.IsStartingUow = false;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
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

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
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
            throw new ArgumentNullException(nameof(dbContextTransaction), "IDbContextTransaction is null");

        return dbContextTransaction != null;
    }
}
