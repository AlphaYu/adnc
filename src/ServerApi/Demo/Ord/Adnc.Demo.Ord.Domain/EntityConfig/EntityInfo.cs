namespace Adnc.Demo.Ord.Domain.EntityConfig;

public class EntityInfo : AbstractDomainEntityInfo
{
    protected override List<Assembly> GetCurrentAssemblies() => [GetType().Assembly, typeof(EventTracker).Assembly];

    protected override void SetTableName(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
            throw new ArgumentNullException(nameof(modelBuilder));

        builder.Entity<EventTracker>().ToTable("ord_eventtracker");
    }
}