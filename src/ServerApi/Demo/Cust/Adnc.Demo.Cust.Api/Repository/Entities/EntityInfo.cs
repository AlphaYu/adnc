namespace Adnc.Demo.Cust.Api.Repository.Entities;

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

        builder.Entity<EventTracker>().ToTable("cust_eventtracker");
    }
}