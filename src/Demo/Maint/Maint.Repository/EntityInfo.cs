namespace Adnc.Demo.Maint.Repository;

public class EntityInfo: AbstractEntityInfo
{
    protected override List<Assembly> GetCurrentAssemblies() => [GetType().Assembly, typeof(EventTracker).Assembly];

    protected override void SetTableName(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        builder.Entity<EventTracker>().ToTable("sys_eventtracker");
    }
}