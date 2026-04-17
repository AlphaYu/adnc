namespace Adnc.Demo.Cust.Api.Repository.Entities;

public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly, typeof(EventTracker).Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventTracker>().ToTable("cust_eventtracker");
        modelBuilder.Entity<Customer>().ToTable("cust_customer");
        modelBuilder.Entity<Finance>().ToTable("cust_finance");
        modelBuilder.Entity<TransactionLog>().ToTable("cust_transactionlog");
    }
}
