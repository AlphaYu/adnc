namespace Adnc.Infra.Repository.EfCore.MySQL.Transaction;

public class MySQLUnitOfWork<TDbContext> : UnitOfWork<TDbContext>
    where TDbContext : MySQLDbContext
{
    public MySQLUnitOfWork(
        TDbContext context
        , ICapPublisher? capPublisher = null)
        : base(context, capPublisher)
    {
    }

    protected override IDbContextTransaction GetDbContextTransaction(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted
        , bool distributed = false)
    {
        if (distributed)
            if (CapPublisher is null)
                throw new ArgumentException("CapPublisher is null");
            else
                return AdncDbContext.Database.BeginTransaction(CapPublisher, false);
        else
            return AdncDbContext.Database.BeginTransaction(isolationLevel);
    }
}