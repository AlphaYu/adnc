namespace Adnc.Infra.Unittest.Reposity.Fixtures.Entities;

public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        //builder.Entity<EventTracker>().ToTable("eventtracker");
    }
}
