namespace Adnc.Demo.Admin.Repository;

public class EntityInfo : AbstractEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly, typeof(EventTracker).Assembly];

    protected override void SetTableName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventTracker>().ToTable("sys_eventtracker");
        modelBuilder.Entity<Organization>().ToTable("sys_organization");
        modelBuilder.Entity<Menu>().ToTable("sys_menu");
        modelBuilder.Entity<Role>().ToTable("sys_role");
        modelBuilder.Entity<RoleMenuRelation>().ToTable("sys_role_menu_relation");
        modelBuilder.Entity<RoleUserRelation>().ToTable("sys_role_user_relation");
        modelBuilder.Entity<User>().ToTable("sys_user");
        modelBuilder.Entity<SysConfig>().ToTable("sys_config");
        modelBuilder.Entity<Dict>().ToTable("sys_dictionary");
        modelBuilder.Entity<DictData>().ToTable("sys_dictionary_data");
        //modelBuilder.Entity<Notice>().ToTable("sys_notice");
    }
}
