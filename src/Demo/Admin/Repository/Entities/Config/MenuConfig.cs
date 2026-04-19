namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class MenuConfig : AbstractEntityTypeConfiguration<Menu>
{
    public override void Configure(EntityTypeBuilder<Menu> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Perm).HasMaxLength(Menu.Code_MaxLength);
        builder.Property(x => x.ParentIds).HasMaxLength(Menu.ParentIds_MaxLength);
        builder.Property(x => x.Component).HasMaxLength(Menu.Component_MaxLength);
        builder.Property(x => x.Icon).HasMaxLength(Menu.Icon_MaxLength);
        builder.Property(x => x.Name).HasMaxLength(Menu.Name_MaxLength);
        builder.Property(x => x.Redirect).HasMaxLength(Menu.Redirect_MaxLength);
        builder.Property(x => x.RouteName).HasMaxLength(Menu.RouteName_MaxLength);
        builder.Property(x => x.RoutePath).HasMaxLength(Menu.RoutePath_MaxLength);
        builder.Property(x => x.Type).HasMaxLength(Menu.Type_MaxLength);
        builder.Property(x => x.Params).HasMaxLength(Menu.Params_MaxLength);
    }
}
