namespace Adnc.Infra.Repository.EfCore.Transaction;

public abstract class UnitOfWork<TDbContext> : IUnitOfWork
    where TDbContext : DbContext
{
    protected TDbContext AdncDbContext { get; init; }
    protected IDbContextTransaction? DbTransaction { get; set; }

    public bool IsStartingUow => AdncDbContext.Database.CurrentTransaction is not null;

    protected UnitOfWork(TDbContext context)
    {
        AdncDbContext = context ?? throw new ArgumentNullException(nameof(context));
    }

    protected abstract IDbContextTransaction GetDbContextTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool distributed = false);

    public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool distributed = false)
    {
        if (AdncDbContext.Database.CurrentTransaction is not null)
            throw new ArgumentException($"UnitOfWork Error,{AdncDbContext.Database.CurrentTransaction}");
        else
            DbTransaction = GetDbContextTransaction(isolationLevel, distributed);
    }

    public void Commit()
    {
        if (DbTransaction is null)
            throw new ArgumentNullException(nameof(DbTransaction), "IDbContextTransaction is null");
        else
            DbTransaction.Commit();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (DbTransaction is null)
            throw new ArgumentNullException(nameof(DbTransaction), "IDbContextTransaction is null");
        else
            await DbTransaction.CommitAsync(cancellationToken);
    }

    public void Rollback()
    {
        if (DbTransaction is null)
            throw new ArgumentNullException(nameof(DbTransaction), "IDbContextTransaction is null");
        else
            DbTransaction.Rollback();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (DbTransaction is null)
            throw new ArgumentNullException(nameof(DbTransaction), "IDbContextTransaction is null");
        else
            await DbTransaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (DbTransaction is not null)
            {
                DbTransaction.Dispose();
                DbTransaction = null;
            }
        }
    }
}