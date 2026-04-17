namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class MenuConfig : AbstractEntityTypeConfiguration<Menu>
{
    public override void Configure(EntityTypeBuilder<Menu> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Perm).HasMaxLength(MenuConsts.Code_MaxLength);
        builder.Property(x => x.ParentIds).HasMaxLength(MenuConsts.ParentIds_MaxLength);
        builder.Property(x => x.Component).HasMaxLength(MenuConsts.Component_MaxLength);
        builder.Property(x => x.Icon).HasMaxLength(MenuConsts.Icon_MaxLength);
        builder.Property(x => x.Name).HasMaxLength(MenuConsts.Name_MaxLength);
        builder.Property(x => x.Redirect).HasMaxLength(MenuConsts.Redirect_MaxLength);
        builder.Property(x => x.RouteName).HasMaxLength(MenuConsts.RouteName_MaxLength);
        builder.Property(x => x.RoutePath).HasMaxLength(MenuConsts.RoutePath_MaxLength);
        builder.Property(x => x.Type).HasMaxLength(MenuConsts.Type_MaxLength);
        builder.Property(x => x.Params).HasMaxLength(MenuConsts.Params_MaxLength);
    }
}
