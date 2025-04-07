using Microsoft.Extensions.Logging;

namespace Adnc.Infra.Repository.EfCore.SqlServer.Transaction;

public class SqlServerUnitOfWork<TDbContext>(TDbContext context, ILogger<SqlServerUnitOfWork<TDbContext>>? logger, ICapPublisher? publisher = null) : UnitOfWork<TDbContext>(context, logger)
    where TDbContext : SqlServerDbContext
{
    private readonly ICapPublisher? _publisher = publisher;

    protected override IDbContextTransaction GetDbContextTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool distributed = false)
    {
        if (distributed)
        {
            if (_publisher is null)
            {
                throw new ArgumentException("CapPublisher is null");
            }
            else
            {
                return AdncDbContext.Database.BeginTransaction(isolationLevel, _publisher, false);
            }
        }
        else
        {
            return AdncDbContext.Database.BeginTransaction(isolationLevel);
        }
    }
}
