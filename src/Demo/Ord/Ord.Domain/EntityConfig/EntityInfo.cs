namespace Adnc.Demo.Ord.Domain.EntityConfig;

public class EntityInfo : AbstractDomainEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly, typeof(EventTracker).Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventTracker>().ToTable("ord_eventtracker");
    }
}
