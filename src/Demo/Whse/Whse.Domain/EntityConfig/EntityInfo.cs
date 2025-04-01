namespace Adnc.Demo.Whse.Domain.EntityConfig;

public class EntityInfo : AbstractDomainEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly, typeof(EventTracker).Assembly];

    protected override void SetTableName(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        builder.Entity<EventTracker>().ToTable("whse_eventtracker");
    }
}