namespace Adnc.Demo.Maint.Repository;

public class EntityInfo : AbstracSharedEntityInfo
{
    public EntityInfo(UserContext userContext) : base(userContext)
    {
    }

    protected override Assembly GetCurrentAssembly() => GetType().Assembly;

    protected override void SetTableName(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
            throw new ArgumentNullException(nameof(modelBuilder));

        builder.Entity<EventTracker>().ToTable("sys_eventtracker");
        builder.Entity<Cfg>().ToTable("sys_config");
        builder.Entity<Dict>().ToTable("sys_dictionary");
        builder.Entity<Notice>().ToTable("sys_notice");
    }
}