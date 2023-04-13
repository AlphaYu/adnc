namespace Adnc.Demo.Usr.Repository;

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
        builder.Entity<Organization>().ToTable("sys_organization");
        builder.Entity<Menu>().ToTable("sys_menu");
        builder.Entity<Role>().ToTable("sys_role");
        builder.Entity<RoleRelation>().ToTable("sys_rolerelation");
        builder.Entity<User>().ToTable("sys_user");
    }
}