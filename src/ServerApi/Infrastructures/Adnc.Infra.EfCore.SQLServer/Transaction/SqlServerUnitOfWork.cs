namespace Adnc.Infra.Repository.EfCore.SqlServer.Transaction;

public class SqlServerUnitOfWork<TDbContext> : UnitOfWork<TDbContext>
    where TDbContext : SqlServerDbContext
{
    private ICapPublisher? _publisher;

    public SqlServerUnitOfWork(
        TDbContext context
        , ICapPublisher? publisher = null)
        : base(context)
    {
        _publisher = publisher;
    }

    protected override IDbContextTransaction GetDbContextTransaction(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted
        , bool distributed = false)
    {
        if (distributed)
            if (_publisher is null)
                throw new ArgumentException("CapPublisher is null");
            else
                return AdncDbContext.Database.BeginTransaction(_publisher, false);
        else
            return AdncDbContext.Database.BeginTransaction(isolationLevel);
    }
}