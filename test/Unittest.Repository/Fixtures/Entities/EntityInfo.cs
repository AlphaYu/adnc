namespace Adnc.Infra.Unittest.Reposity.Fixtures.Entities;

public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetCurrentAssemblies() => [GetType().Assembly];

    protected override void SetTableName(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
            throw new ArgumentNullException(nameof(modelBuilder));

        //builder.Entity<EventTracker>().ToTable("eventtracker");
    }
}