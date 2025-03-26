namespace Adnc.Demo.Cust.Api.Repository.Entities;

public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetCurrentAssemblies() => [GetType().Assembly, typeof(EventTracker).Assembly];

    protected override void SetTableName(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        builder.Entity<EventTracker>().ToTable("cust_eventtracker");
        builder.Entity<Customer>().ToTable("cust_customer");
        builder.Entity<Finance>().ToTable("cust_finance");
        builder.Entity<TransactionLog>().ToTable("cust_transactionlog");
    }
}