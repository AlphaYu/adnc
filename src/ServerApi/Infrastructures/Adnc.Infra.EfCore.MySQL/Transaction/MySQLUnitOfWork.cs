namespace Adnc.Infra.Repository.EfCore.MySQL.Transaction;

public class MySQLUnitOfWork<TDbContext> : UnitOfWork<TDbContext>
    where TDbContext : MySQLDbContext
{
    private ICapPublisher? _publisher;

    public MySQLUnitOfWork(
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