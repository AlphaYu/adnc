namespace Adnc.Infra.Repository.EfCore.MongoDB.Transaction;

//todo: implement MongoDbUnitOfWork
//public class MongoDbUnitOfWork<TDbContext>(TDbContext context, ICapPublisher? publisher = null) : UnitOfWork<TDbContext>(context)
//    where TDbContext : SqlServerDbContext
//{
//    private ICapPublisher? _publisher = publisher;

//    protected override IDbContextTransaction GetDbContextTransaction(
//        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted
//        , bool distributed = false)
//    {
//        if (distributed)
//            if (_publisher is null)
//                throw new ArgumentException("CapPublisher is null");
//            else
//                return AdncDbContext.Database.BeginTransaction(_publisher, false);
//        else
//            return AdncDbContext.Database.BeginTransaction(isolationLevel);
//    }
//}
